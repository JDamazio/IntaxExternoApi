using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Enums;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class ExclusaoIcmsService : IExclusaoIcmsService
{
    private readonly IExclusaoIcmsResultadoRepository _resultadoRepository;
    private readonly IOportunidadeRepository _oportunidadeRepository;
    private readonly ISpedContribuicoesRepository _spedContribuicoesRepository;
    private readonly ISpedFiscalRepository _spedFiscalRepository;

    // Listas de CFOPs e CSTs para classificação
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

    private static readonly List<string> CstSaida = new()
    {
        "04", "05", "06", "07", "08", "09", "49"
    };

    private static readonly List<string> CstEntradaPositivo = new()
    {
        "50", "52", "54", "56", "60", "62", "63", "64", "66", "67", "75", "98", "99"
    };

    private static readonly List<string> CstEntradaNegativo = new()
    {
        "51", "53", "55", "61", "65", "70", "71", "72", "73", "74"
    };

    private static readonly List<string> CstFiscal = new()
    {
        "000", "010", "020", "100", "110", "120", "200", "210", "220", "300", "310", "320",
        "400", "410", "420", "500", "510", "520", "600", "610", "620", "700", "710", "720",
        "800", "810", "820"
    };

    public ExclusaoIcmsService(
        IExclusaoIcmsResultadoRepository resultadoRepository,
        IOportunidadeRepository oportunidadeRepository,
        ISpedContribuicoesRepository spedContribuicoesRepository,
        ISpedFiscalRepository spedFiscalRepository)
    {
        _resultadoRepository = resultadoRepository;
        _oportunidadeRepository = oportunidadeRepository;
        _spedContribuicoesRepository = spedContribuicoesRepository;
        _spedFiscalRepository = spedFiscalRepository;
    }

    /// <summary>
    /// Obtém as alíquotas de PIS e COFINS baseado no regime tributário
    /// </summary>
    private static (decimal AliqPis, decimal AliqCofins) ObterAliquotasPorRegime(RegimeTributario regime)
    {
        return regime switch
        {
            RegimeTributario.LucroReal => (1.65m, 7.6m),           // Lucro Real
            RegimeTributario.LucroPresumido => (0.65m, 3.0m),      // Lucro Presumido
            RegimeTributario.SimplesNacional => (0m, 0m),          // Simples Nacional - não se aplica
            _ => (0.65m, 3.0m)                                      // Default: Lucro Presumido
        };
    }

    public async Task<Response<List<ExclusaoIcmsResultadoDto>>> CalcularExclusaoAsync(CalcularExclusaoRequestDto request)
    {
        try
        {
            // Buscar a oportunidade e o cliente para obter o regime tributário
            var oportunidade = await _oportunidadeRepository.GetByIdAsync(request.OportunidadeId);

            if (oportunidade == null)
                return new Response<List<ExclusaoIcmsResultadoDto>>(
                    false,
                    "Oportunidade não encontrada",
                    404
                );

            var regimeTributario = oportunidade.Cliente?.RegimeTributario ?? RegimeTributario.LucroPresumido;

            var resultadoCalculado = CalcularExclusao(request, regimeTributario);

            // Salvar dados de entrada (SPED Contribuições e Fiscal)
            await SalvarDadosEntradaAsync(request, oportunidade.CreatedBy ?? "system");

            // Salvar resultado no banco
            var entidades = resultadoCalculado.Select(r => new ExclusaoIcmsResultado
            {
                OportunidadeId = request.OportunidadeId,
                DataInicial = r.DataInicial,
                ValorIcms = r.ValorIcms,
                ValorPis = r.ValorPis,
                ValorCofins = r.ValorCofins,
                ValorPisCofins = r.ValorPisCofins
            }).ToList();

            await _resultadoRepository.SaveResultadosAsync(entidades, request.OportunidadeId);

            return new Response<List<ExclusaoIcmsResultadoDto>>(
                true,
                "Cálculo realizado com sucesso",
                resultadoCalculado,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<ExclusaoIcmsResultadoDto>>(
                false,
                $"Erro ao calcular exclusão: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<List<ExclusaoIcmsResultadoDto>>> GetResultadoAsync(int oportunidadeId)
    {
        try
        {
            var resultados = await _resultadoRepository.GetByOportunidadeIdAsync(oportunidadeId);

            if (!resultados.Any())
                return new Response<List<ExclusaoIcmsResultadoDto>>(
                    false,
                    "Nenhum resultado encontrado para esta Oportunidade",
                    404
                );

            var dtos = resultados.Select(r => new ExclusaoIcmsResultadoDto
            {
                DataInicial = r.DataInicial,
                ValorIcms = r.ValorIcms,
                ValorPis = r.ValorPis,
                ValorCofins = r.ValorCofins,
                ValorPisCofins = r.ValorPisCofins
            }).ToList();

            return new Response<List<ExclusaoIcmsResultadoDto>>(
                true,
                "Sucesso",
                dtos,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<ExclusaoIcmsResultadoDto>>(
                false,
                $"Erro ao buscar resultado: {ex.Message}",
                500
            );
        }
    }

    /// <summary>
    /// Lógica principal de cálculo de exclusão de ICMS
    /// </summary>
    private List<ExclusaoIcmsResultadoDto> CalcularExclusao(CalcularExclusaoRequestDto request, RegimeTributario regimeTributario)
    {
        // Obter alíquotas baseado no regime tributário
        var (aliqPis, aliqCofins) = ObterAliquotasPorRegime(regimeTributario);

        // ===== ETAPA 1: PROCESSAR CONTRIBUIÇÕES =====
        var aliqContribuicoes = ProcessarContribuicoes(request.DadosContribuicoes, aliqPis, aliqCofins);

        // Agrupar contribuições por mês/ano
        var contribuicoesAgrupados = aliqContribuicoes
            .GroupBy(a => new { Year = a.DataInicial?.Year ?? 0, Month = a.DataInicial?.Month ?? 0 })
            .Select(g => new ContribuicoesAgrupadas
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                AliqPis = g.FirstOrDefault()?.AliqPis,
                AliqCofins = g.FirstOrDefault()?.AliqCofins,
                ValorTotalIcms = g.Sum(a => a.ValorIcms),
                ValorTotalPis = g.Sum(a => a.ValorPis),
                ValorTotalCofins = g.Sum(a => a.ValorCofins),
                ValorTotalPisCofins = g.Sum(a => a.ValorPisCofins)
            })
            .OrderBy(a => a.Ano).ThenBy(a => a.Mes)
            .ToList();

        // ===== ETAPA 2: PROCESSAR FISCAL =====
        var aliqFiscal = ProcessarFiscal(request.DadosFiscais, contribuicoesAgrupados, aliqPis, aliqCofins);

        // Agrupar fiscal por mês/ano
        var fiscaisAgrupados = aliqFiscal
            .GroupBy(a => new { Year = a.DataInicial?.Year ?? 0, Month = a.DataInicial?.Month ?? 0 })
            .Select(g => new
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                ValorTotalIcms = g.Sum(a => a.ValorIcms),
                ValorTotalPis = g.Sum(a => a.ValorPis),
                ValorTotalCofins = g.Sum(a => a.ValorCofins),
                ValorTotalPisCofins = g.Sum(a => a.ValorPisCofins)
            })
            .OrderBy(a => a.Ano).ThenBy(a => a.Mes)
            .ToList();

        // ===== ETAPA 3: CALCULAR RESULTADO FINAL (FISCAL - CONTRIBUIÇÕES) =====
        var resultado = fiscaisAgrupados.GroupJoin(
            contribuicoesAgrupados,
            f => new { f.Ano, f.Mes },
            c => new { c.Ano, c.Mes },
            (f, grupoContribuicoes) => new ExclusaoIcmsResultadoDto
            {
                DataInicial = DateTime.SpecifyKind(new DateTime(f.Ano, f.Mes, 1), DateTimeKind.Utc),
                ValorIcms = Math.Max(0, (f.ValorTotalIcms ?? 0) - grupoContribuicoes.Sum(c => c.ValorTotalIcms ?? 0)),
                ValorPis = Math.Max(0, (f.ValorTotalPis ?? 0) - grupoContribuicoes.Sum(c => c.ValorTotalPis ?? 0)),
                ValorCofins = Math.Max(0, (f.ValorTotalCofins ?? 0) - grupoContribuicoes.Sum(c => c.ValorTotalCofins ?? 0)),
                ValorPisCofins = Math.Max(0, (f.ValorTotalPisCofins ?? 0) - grupoContribuicoes.Sum(c => c.ValorTotalPisCofins ?? 0))
            })
            .Where(r => r.ValorIcms > 0) // Remove registros com ICMS = 0
            .ToList();

        return resultado;
    }

    /// <summary>
    /// Processa dados de contribuições (SPED Contribuições)
    /// </summary>
    private List<AliquotasContribuicoesCalc> ProcessarContribuicoes(
        List<SpedContribuicoesDto> contribuicoes,
        decimal aliqPisRegime,
        decimal aliqCofinsRegime)
    {
        var resultado = new List<AliquotasContribuicoesCalc>();

        foreach (var contrib in contribuicoes)
        {
            // Usar as alíquotas do regime tributário do cliente
            var aliqPis = contrib.AliqPis ?? aliqPisRegime;
            var aliqCofins = contrib.AliqCofins ?? aliqCofinsRegime;

            // Operações de ENTRADA (Devoluções)
            if (CfopDevolucao.Contains(contrib.CodFiscal))
            {
                if (CstEntradaPositivo.Contains(contrib.CodSitPis))
                {
                    resultado.Add(new AliquotasContribuicoesCalc
                    {
                        AliqPis = aliqPis,
                        AliqCofins = aliqCofins,
                        ValorIcms = contrib.ValorIcms,
                        ValorPis = contrib.ValorIcms * aliqPis / 100,
                        ValorCofins = contrib.ValorIcms * aliqCofins / 100,
                        ValorPisCofins = (contrib.ValorIcms * aliqPis / 100) + (contrib.ValorIcms * aliqCofins / 100),
                        DataInicial = contrib.DataInicial
                    });
                }
                else if (CstEntradaNegativo.Contains(contrib.CodSitPis))
                {
                    resultado.Add(new AliquotasContribuicoesCalc
                    {
                        AliqPis = aliqPis,
                        AliqCofins = aliqCofins,
                        ValorIcms = contrib.ValorIcms * -1,
                        ValorPis = contrib.ValorIcms * aliqPis / 100 * -1,
                        ValorCofins = contrib.ValorIcms * aliqCofins / 100 * -1,
                        ValorPisCofins = (contrib.ValorIcms * aliqPis / 100 * -1) + (contrib.ValorIcms * aliqCofins / 100 * -1),
                        DataInicial = contrib.DataInicial
                    });
                }
            }
            // Operações de SAÍDA (Faturamento)
            else if (CfopFaturamento.Contains(contrib.CodFiscal) && CstSaida.Contains(contrib.CodSitPis))
            {
                resultado.Add(new AliquotasContribuicoesCalc
                {
                    AliqPis = aliqPis,
                    AliqCofins = aliqCofins,
                    ValorIcms = contrib.ValorIcms,
                    ValorPis = contrib.ValorIcms * aliqPis / 100,
                    ValorCofins = contrib.ValorIcms * aliqCofins / 100,
                    ValorPisCofins = (contrib.ValorIcms * aliqPis / 100) + (contrib.ValorIcms * aliqCofins / 100),
                    DataInicial = contrib.DataInicial
                });
            }
        }

        return resultado;
    }

    /// <summary>
    /// Processa dados fiscais (SPED Fiscal)
    /// </summary>
    private List<AliquotasFiscalCalc> ProcessarFiscal(
        List<SpedFiscalDto> fiscais,
        List<ContribuicoesAgrupadas> contribuicoesAgrupados,
        decimal aliqPisRegime,
        decimal aliqCofinsRegime)
    {
        var resultado = new List<AliquotasFiscalCalc>();

        foreach (var fiscal in fiscais)
        {
            // Filtrar apenas CFOPs de faturamento e CSTs válidos
            if (CstFiscal.Contains(fiscal.CstIcms) && CfopFaturamento.Contains(fiscal.Cfop))
            {
                // Usar alíquotas do regime tributário do cliente
                decimal aliqPis = aliqPisRegime;
                decimal aliqCofins = aliqCofinsRegime;

                // Tenta buscar das contribuições do mesmo período (se houver)
                if (fiscal.DataInicial.HasValue)
                {
                    var contribuicaoMesmoMes = contribuicoesAgrupados.FirstOrDefault(c =>
                        c.Ano == fiscal.DataInicial.Value.Year &&
                        c.Mes == fiscal.DataInicial.Value.Month);

                    if (contribuicaoMesmoMes != null)
                    {
                        // Se as contribuições do mesmo mês tiverem alíquotas, usa elas
                        // Caso contrário, mantém as do regime
                        aliqPis = contribuicaoMesmoMes.AliqPis ?? aliqPisRegime;
                        aliqCofins = contribuicaoMesmoMes.AliqCofins ?? aliqCofinsRegime;
                    }
                }

                resultado.Add(new AliquotasFiscalCalc
                {
                    AliqPis = aliqPis,
                    AliqCofins = aliqCofins,
                    ValorIcms = fiscal.ValorIcms,
                    ValorPis = fiscal.ValorIcms * aliqPis / 100,
                    ValorCofins = fiscal.ValorIcms * aliqCofins / 100,
                    ValorPisCofins = (fiscal.ValorIcms * aliqPis / 100) + (fiscal.ValorIcms * aliqCofins / 100),
                    DataInicial = fiscal.DataInicial
                });
            }
        }

        return resultado;
    }

    // Classes auxiliares para cálculo
    private class ContribuicoesAgrupadas
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal? AliqPis { get; set; }
        public decimal? AliqCofins { get; set; }
        public decimal? ValorTotalIcms { get; set; }
        public decimal? ValorTotalPis { get; set; }
        public decimal? ValorTotalCofins { get; set; }
        public decimal? ValorTotalPisCofins { get; set; }
    }

    private class AliquotasContribuicoesCalc
    {
        public decimal? AliqPis { get; set; }
        public decimal? AliqCofins { get; set; }
        public decimal? ValorIcms { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorCofins { get; set; }
        public decimal? ValorPisCofins { get; set; }
        public DateTime? DataInicial { get; set; }
    }

    private class AliquotasFiscalCalc
    {
        public decimal? AliqPis { get; set; }
        public decimal? AliqCofins { get; set; }
        public decimal? ValorIcms { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorCofins { get; set; }
        public decimal? ValorPisCofins { get; set; }
        public DateTime? DataInicial { get; set; }
    }

    /// <summary>
    /// Salva os dados de entrada (SPED Contribuições e Fiscal) no banco
    /// </summary>
    private async Task SalvarDadosEntradaAsync(CalcularExclusaoRequestDto request, string userId)
    {
        // Deletar dados antigos desta oportunidade
        await _spedContribuicoesRepository.DeleteByOportunidadeIdAsync(request.OportunidadeId, userId);
        await _spedFiscalRepository.DeleteByOportunidadeIdAsync(request.OportunidadeId, userId);

        // Salvar SPED Contribuições
        if (request.DadosContribuicoes.Any())
        {
            var contribuicoesEntities = request.DadosContribuicoes.Select(c => new SpedContribuicoes
            {
                OportunidadeId = request.OportunidadeId,
                CodFiscal = c.CodFiscal,
                CodSitPis = c.CodSitPis,
                AliqPis = c.AliqPis,
                AliqCofins = c.AliqCofins,
                ValorIcms = c.ValorIcms,
                DataInicial = c.DataInicial,
                Regime = c.Regime
            }).ToList();

            await _spedContribuicoesRepository.CreateManyAsync(contribuicoesEntities, userId);
        }

        // Salvar SPED Fiscal
        if (request.DadosFiscais.Any())
        {
            var fiscaisEntities = request.DadosFiscais.Select(f => new SpedFiscal
            {
                OportunidadeId = request.OportunidadeId,
                Cfop = f.Cfop,
                CstIcms = f.CstIcms,
                ValorIcms = f.ValorIcms,
                DataInicial = f.DataInicial
            }).ToList();

            await _spedFiscalRepository.CreateManyAsync(fiscaisEntities, userId);
        }
    }

    /// <summary>
    /// Busca os dados de entrada salvos (SPED Contribuições e Fiscal) de uma oportunidade
    /// </summary>
    public async Task<Response<CalcularExclusaoRequestDto>> GetDadosEntradaAsync(int oportunidadeId)
    {
        try
        {
            var contribuicoes = await _spedContribuicoesRepository.GetByOportunidadeIdAsync(oportunidadeId);
            var fiscais = await _spedFiscalRepository.GetByOportunidadeIdAsync(oportunidadeId);

            if (!contribuicoes.Any() && !fiscais.Any())
                return new Response<CalcularExclusaoRequestDto>(
                    false,
                    "Nenhum dado de entrada encontrado para esta Oportunidade",
                    404
                );

            var dadosEntrada = new CalcularExclusaoRequestDto
            {
                OportunidadeId = oportunidadeId,
                DadosContribuicoes = contribuicoes.Select(c => new SpedContribuicoesDto
                {
                    CodFiscal = c.CodFiscal,
                    CodSitPis = c.CodSitPis,
                    AliqPis = c.AliqPis,
                    AliqCofins = c.AliqCofins,
                    ValorIcms = c.ValorIcms,
                    DataInicial = c.DataInicial,
                    Regime = c.Regime
                }).ToList(),
                DadosFiscais = fiscais.Select(f => new SpedFiscalDto
                {
                    Cfop = f.Cfop,
                    CstIcms = f.CstIcms,
                    ValorIcms = f.ValorIcms,
                    DataInicial = f.DataInicial
                }).ToList()
            };

            return new Response<CalcularExclusaoRequestDto>(
                true,
                "Dados de entrada carregados com sucesso",
                dadosEntrada,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<CalcularExclusaoRequestDto>(
                false,
                $"Erro ao buscar dados de entrada: {ex.Message}",
                500
            );
        }
    }
}
