using IntaxExterno.Application.DTOs.ExclusaoIcms;
using System.Globalization;
using System.Text;

namespace IntaxExterno.Application.Services;

public interface ISpedParserService
{
    Task<SpedParseResult> ParseSpedFilesAsync(Stream contribuicoesStream, Stream fiscalStream);
}

public class SpedParserService : ISpedParserService
{
    private static readonly List<string> CfopFaturamento = new()
    {
        "5101", "5102", "5103", "5105", "5106", "5109", "5110", "5111", "5118", "5119",
        "5120", "5122", "5123", "5125", "5129", "5132", "5251", "5252", "5253", "5254",
        "5255", "5256", "5257", "5258", "5301", "5302", "5303", "5304", "5305", "5306",
        "5307", "5351", "5352", "5353", "5354", "5355", "5356", "5357", "5359", "5360",
        "5401", "5402", "5403", "5405", "5651", "5652", "5653", "5654", "5655", "5656",
        "5667", "5922", "5932", "5933", "6101", "6102", "6103", "6105", "6106", "6107",
        "6108", "6109", "6110", "6111", "6118", "6119", "6120", "6122", "6123", "6125",
        "6129", "6132", "6251", "6252", "6253", "6254", "6255", "6256", "6257", "6258",
        "6301", "6302", "6303", "6304", "6305", "6306", "6307", "6351", "6352", "6353",
        "6354", "6355", "6356", "6357", "6359", "6360", "6401", "6402", "6403", "6404",
        "6933", "6651", "6652", "6653", "6654", "6655", "6656", "6667", "6922", "6932",
        "7101", "7102", "7105", "7106", "7127", "7129", "7251", "7301", "7358", "7501",
        "7651", "7654", "7667"
    };

    private static readonly List<string> CfopDevolucao = new()
    {
        "1201", "1202", "1203", "1204", "1205", "1206", "1207", "1212", "1214", "1215",
        "1216", "1410", "1411", "1660", "1661", "1662", "2201", "2202", "2203", "2204",
        "2205", "2206", "2207", "2212", "2214", "2215", "2216", "2410", "2411", "2660",
        "2661", "2662", "3201", "3202", "3205", "3206", "3207", "3211", "3212", "3503"
    };

    public async Task<SpedParseResult> ParseSpedFilesAsync(Stream contribuicoesStream, Stream fiscalStream)
    {
        var result = new SpedParseResult();

        if (contribuicoesStream != null)
        {
            result.DadosContribuicoes = await ParseSpedContribuicoesAsync(contribuicoesStream);
        }

        if (fiscalStream != null)
        {
            result.DadosFiscal = await ParseSpedFiscalAsync(fiscalStream);
        }

        return result;
    }

    /// <summary>
    /// Parse SPED Contribuições (EFD-PIS/COFINS)
    /// </summary>
    private async Task<List<SpedContribuicoesDto>> ParseSpedContribuicoesAsync(Stream stream)
    {
        var dados = new List<SpedContribuicoesDto>();
        string? regime = null;
        DateTime? dataInicial = null;

        using var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var fields = line.Split('|');
            if (fields.Length < 2) continue;

            var registro = fields[1].Trim();

            try
            {
                switch (registro)
                {
                    case "0000": // Cabeçalho
                        if (fields.Length > 5)
                        {
                            var dtIniStr = fields[5].Trim();
                            if (!string.IsNullOrEmpty(dtIniStr) && dtIniStr.Length == 8)
                            {
                                try
                                {
                                    var dia = int.Parse(dtIniStr.Substring(0, 2));
                                    var mes = int.Parse(dtIniStr.Substring(2, 2));
                                    var ano = int.Parse(dtIniStr.Substring(4, 4));
                                    dataInicial = DateTime.SpecifyKind(new DateTime(ano, mes, dia), DateTimeKind.Utc);
                                }
                                catch
                                {
                                    // Ignora erro de parse de data
                                }
                            }
                        }
                        break;

                    case "0110": // Regime de Apuração
                        if (fields.Length > 2)
                        {
                            regime = fields[2].Trim();
                        }
                        break;

                    case "C170": // Itens do Documento (Nota Fiscal)
                        // |C170|NUM_ITEM|COD_ITEM|DESCR_COMPL|QTD|UNID|VL_ITEM|VL_DESC|IND_MOV|CST_ICMS|CFOP|COD_NAT|VL_BC_ICMS|ALIQ_ICMS|VL_ICMS|VL_BC_ICMS_ST|ALIQ_ST|VL_ICMS_ST|IND_APUR|CST_PIS|VL_BC_PIS|ALIQ_PIS|QUANT_BC_PIS|ALIQ_PIS_QUANT|VL_PIS|CST_COFINS|VL_BC_COFINS|ALIQ_COFINS|QUANT_BC_COFINS|ALIQ_COFINS_QUANT|VL_COFINS|COD_CTA|
                        if (fields.Length >= 28)
                        {
                            var cfop = GetField(fields, 11);
                            var cstPis = GetField(fields, 20);

                            if (string.IsNullOrEmpty(cfop) || string.IsNullOrEmpty(cstPis))
                                continue;

                            // Verificar se é CFOP relevante
                            if (!CfopFaturamento.Contains(cfop) && !CfopDevolucao.Contains(cfop))
                                continue;

                            var aliqPis = ParseDecimal(GetField(fields, 22)); // ALIQ_PIS
                            var aliqCofins = ParseDecimal(GetField(fields, 28)); // ALIQ_COFINS
                            var vlPis = ParseDecimal(GetField(fields, 25)); // VL_PIS
                            var vlCofins = ParseDecimal(GetField(fields, 31)); // VL_COFINS
                            var vlItem = ParseDecimal(GetField(fields, 7)); // VL_ITEM

                            // Calcula ICMS estimado se não tiver valor direto
                            decimal? valorIcms = vlItem * 0.18m; // Alíquota ICMS estimada

                            dados.Add(new SpedContribuicoesDto
                            {
                                CodFiscal = cfop,
                                CodSitPis = cstPis,
                                AliqPis = aliqPis ?? 0,
                                AliqCofins = aliqCofins ?? 0,
                                ValorIcms = valorIcms,
                                DataInicial = dataInicial,
                                Regime = regime ?? "2" // Default: Não-Cumulativo
                            });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log ou ignora erro de linha específica
                Console.WriteLine($"Erro ao processar linha SPED Contribuições: {ex.Message}");
                continue;
            }
        }

        return dados;
    }

    /// <summary>
    /// Obtém campo do array de forma segura
    /// </summary>
    private string GetField(string[] fields, int index)
    {
        return index < fields.Length ? fields[index].Trim() : string.Empty;
    }

    /// <summary>
    /// Parse SPED Fiscal (EFD-ICMS/IPI)
    /// </summary>
    private async Task<List<SpedFiscalDto>> ParseSpedFiscalAsync(Stream stream)
    {
        var dados = new List<SpedFiscalDto>();
        DateTime? dataInicial = null;

        using var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var fields = line.Split('|');
            if (fields.Length < 2) continue;

            var registro = fields[1].Trim();

            try
            {
                switch (registro)
                {
                    case "0000": // Cabeçalho
                        if (fields.Length > 5)
                        {
                            var dtIniStr = GetField(fields, 5);
                            if (!string.IsNullOrEmpty(dtIniStr) && dtIniStr.Length == 8)
                            {
                                try
                                {
                                    var dia = int.Parse(dtIniStr.Substring(0, 2));
                                    var mes = int.Parse(dtIniStr.Substring(2, 2));
                                    var ano = int.Parse(dtIniStr.Substring(4, 4));
                                    dataInicial = DateTime.SpecifyKind(new DateTime(ano, mes, dia), DateTimeKind.Utc);
                                }
                                catch
                                {
                                    // Ignora erro de parse de data
                                }
                            }
                        }
                        break;

                    case "C170": // Itens do Documento (Nota Fiscal)
                        // |C170|NUM_ITEM|COD_ITEM|DESCR_COMPL|QTD|UNID|VL_ITEM|VL_DESC|IND_MOV|CST_ICMS|CFOP|COD_NAT|VL_BC_ICMS|ALIQ_ICMS|VL_ICMS|VL_BC_ICMS_ST|ALIQ_ST|VL_ICMS_ST|IND_APUR|CST_IPI|VL_BC_IPI|ALIQ_IPI|VL_IPI|
                        if (fields.Length >= 16)
                        {
                            var cstIcms = GetField(fields, 10); // CST_ICMS
                            var cfop = GetField(fields, 11); // CFOP
                            var vlIcms = ParseDecimal(GetField(fields, 15)); // VL_ICMS

                            if (string.IsNullOrEmpty(cfop))
                                continue;

                            // Filtrar apenas CFOPs de faturamento
                            if (!CfopFaturamento.Contains(cfop))
                                continue;

                            dados.Add(new SpedFiscalDto
                            {
                                Cfop = cfop,
                                CstIcms = cstIcms,
                                ValorIcms = vlIcms ?? 0,
                                DataInicial = dataInicial
                            });
                        }
                        break;

                    case "C190": // Registro Analítico (Totalização)
                        // |C190|CST_ICMS|CFOP|ALIQ_ICMS|VL_OPR|VL_BC_ICMS|VL_ICMS|VL_BC_ICMS_ST|VL_ICMS_ST|VL_RED_BC|VL_IPI|COD_OBS|
                        if (fields.Length >= 8)
                        {
                            var cstIcms = GetField(fields, 2); // CST_ICMS
                            var cfop = GetField(fields, 3); // CFOP
                            var vlIcms = ParseDecimal(GetField(fields, 7)); // VL_ICMS

                            if (string.IsNullOrEmpty(cfop))
                                continue;

                            if (!CfopFaturamento.Contains(cfop))
                                continue;

                            dados.Add(new SpedFiscalDto
                            {
                                Cfop = cfop,
                                CstIcms = cstIcms,
                                ValorIcms = vlIcms ?? 0,
                                DataInicial = dataInicial
                            });
                        }
                        break;

                    case "C320": // Totalização por CST
                        // |C320|CST_ICMS|CFOP|ALIQ_ICMS|VL_OPR|VL_BC_ICMS|VL_ICMS|VL_BC_ICMS_ST|VL_ICMS_ST|VL_RED_BC|COD_OBS|
                        if (fields.Length >= 7)
                        {
                            var cstIcms = GetField(fields, 2);
                            var cfop = GetField(fields, 3);
                            var vlIcms = ParseDecimal(GetField(fields, 7));

                            if (string.IsNullOrEmpty(cfop))
                                continue;

                            if (!CfopFaturamento.Contains(cfop))
                                continue;

                            dados.Add(new SpedFiscalDto
                            {
                                Cfop = cfop,
                                CstIcms = cstIcms,
                                ValorIcms = vlIcms ?? 0,
                                DataInicial = dataInicial
                            });
                        }
                        break;

                    case "D190": // Prestação de Serviços - Totalização
                        // |D190|CST_ICMS|CFOP|ALIQ_ICMS|VL_OPR|VL_BC_ICMS|VL_ICMS|VL_BC_ICMS_ST|VL_ICMS_ST|VL_RED_BC|COD_OBS|
                        if (fields.Length >= 8)
                        {
                            var cstIcms = GetField(fields, 2); // CST_ICMS
                            var cfop = GetField(fields, 3); // CFOP
                            var vlIcms = ParseDecimal(GetField(fields, 7)); // VL_ICMS

                            if (string.IsNullOrEmpty(cfop))
                                continue;

                            if (!CfopFaturamento.Contains(cfop))
                                continue;

                            dados.Add(new SpedFiscalDto
                            {
                                Cfop = cfop,
                                CstIcms = cstIcms,
                                ValorIcms = vlIcms ?? 0,
                                DataInicial = dataInicial
                            });
                        }
                        break;

                    case "D300": // Documentos de Transporte
                        // |D300|COD_MOD|SER|SUB|NUM_DOC|DT_DOC|VL_DOC|VL_DESC|VL_SERV|VL_BC_ICMS|VL_ICMS|VL_NT|COD_INF|COD_CTA|
                        if (fields.Length >= 11)
                        {
                            var vlIcms = ParseDecimal(GetField(fields, 11)); // VL_ICMS

                            // Usa CFOP padrão para transporte
                            dados.Add(new SpedFiscalDto
                            {
                                Cfop = "5353", // CFOP padrão transporte
                                CstIcms = "000",
                                ValorIcms = vlIcms ?? 0,
                                DataInicial = dataInicial
                            });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log ou ignora erro de linha específica
                Console.WriteLine($"Erro ao processar linha SPED Fiscal: {ex.Message}");
                continue;
            }
        }

        return dados;
    }

    /// <summary>
    /// Converte string para decimal usando cultura invariante
    /// </summary>
    private decimal? ParseDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        // Remove espaços e substitui vírgula por ponto
        value = value.Trim().Replace(',', '.');

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        return null;
    }
}

public class SpedParseResult
{
    public List<SpedContribuicoesDto> DadosContribuicoes { get; set; } = new();
    public List<SpedFiscalDto> DadosFiscal { get; set; } = new();
}
