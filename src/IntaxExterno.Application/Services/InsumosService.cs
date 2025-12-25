using IntaxExterno.Application.DTOs.Insumos;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Enums;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IntaxExterno.Application.Services;

public class InsumosService : IInsumosService
{
    private readonly IInsumosResultadoRepository _insumosResultadoRepository;
    private readonly ISpedContabilI050Repository _spedContabilI050Repository;
    private readonly ISpedContabilI250Repository _spedContabilI250Repository;
    private readonly ISpedContabilI155Repository _spedContabilI155Repository;
    private readonly IOportunidadeRepository _oportunidadeRepository;
    private readonly ISpedParserService _spedParserService;

    public InsumosService(
        IInsumosResultadoRepository insumosResultadoRepository,
        ISpedContabilI050Repository spedContabilI050Repository,
        ISpedContabilI250Repository spedContabilI250Repository,
        ISpedContabilI155Repository spedContabilI155Repository,
        IOportunidadeRepository oportunidadeRepository,
        ISpedParserService spedParserService)
    {
        _insumosResultadoRepository = insumosResultadoRepository;
        _spedContabilI050Repository = spedContabilI050Repository;
        _spedContabilI250Repository = spedContabilI250Repository;
        _spedContabilI155Repository = spedContabilI155Repository;
        _oportunidadeRepository = oportunidadeRepository;
        _spedParserService = spedParserService;
    }

    public async Task<Response<bool>> ProcessarSpedContabilAsync(int oportunidadeId, Stream arquivo)
    {
        try
        {
            // Parse do arquivo SPED Contábil
            var parseResult = await _spedParserService.ParseSpedContabilAsync(arquivo);

            if (!parseResult.PlanoContas.Any())
                return new Response<bool>(
                    false,
                    "Arquivo SPED Contábil não contém dados válidos (I050)",
                    400
                );

            // Converter DTOs para entidades e salvar Plano de Contas (I050)
            var planoContasEntities = parseResult.PlanoContas.Select(dto => new SpedContabilI050
            {
                OportunidadeId = oportunidadeId,
                CodigoCta = dto.CodigoCta,
                NomeCta = dto.NomeCta,
                CodNatureza = dto.CodNatureza,
                DataInicial = dto.DataInicial,
                DataFinal = dto.DataFinal,
                Status = 0 // Pendente
            }).ToList();

            await _spedContabilI050Repository.SaveAsync(planoContasEntities, oportunidadeId);

            // Converter DTOs para entidades e salvar Lançamentos (I250)
            var insumosEntities = parseResult.Insumos.Select(dto => new SpedContabilI250
            {
                OportunidadeId = oportunidadeId,
                CodigoCta = dto.CodigoCta,
                DataApuracao = dto.DataApuracao,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Situacao = 0, // Ativo
                IndicadorDC = dto.IndicadorDC
            }).ToList();

            await _spedContabilI250Repository.SaveAsync(insumosEntities, oportunidadeId);

            // Converter DTOs para entidades e salvar Saldos (I155)
            var saldosEntities = parseResult.Saldos.Select(dto => new SpedContabilI155
            {
                OportunidadeId = oportunidadeId,
                CodCta = dto.CodCta,
                CodCcus = dto.CodCcus,
                ValorDebito = dto.ValorDebito,
                ValorCredito = dto.ValorCredito,
                IndicadorSituacao = dto.IndicadorSituacao,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            }).ToList();

            await _spedContabilI155Repository.SaveAsync(saldosEntities, oportunidadeId);

            // Atualizar contadores de I250 por conta no I050
            var contadoresPorConta = insumosEntities
                .GroupBy(i => i.CodigoCta)
                .Select(g => new
                {
                    CodigoCta = g.Key,
                    QtdTotal = g.Count(),
                    QtdAtivos = g.Count(x => x.Situacao == 0)
                })
                .ToList();

            foreach (var conta in planoContasEntities)
            {
                var contador = contadoresPorConta.FirstOrDefault(c => c.CodigoCta == conta.CodigoCta);
                if (contador != null)
                {
                    conta.QtdI250 = contador.QtdTotal;
                    conta.QtdI250Selecionados = contador.QtdAtivos;
                }
            }

            return new Response<bool>(
                true,
                $"SPED Contábil processado com sucesso. {planoContasEntities.Count} contas, {insumosEntities.Count} lançamentos e {saldosEntities.Count} saldos importados",
                true,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<bool>(
                false,
                $"Erro ao processar SPED Contábil: {ex.Message}",
                500
            );
        }
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

    public async Task<Response<List<InsumosResultadoDto>>> CalcularInsumosAsync(CalcularInsumosRequestDto request)
    {
        try
        {
            // Validar modalidade
            if (request.Modalidade != 1.0m && request.Modalidade != 0.7m)
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Modalidade inválida. Use 1.0 para 100% ou 0.7 para 70%",
                    400
                );

            // Buscar a oportunidade e o cliente para obter o regime tributário
            var oportunidade = await _oportunidadeRepository.GetByIdAsync(request.OportunidadeId);

            if (oportunidade == null)
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Oportunidade não encontrada",
                    404
                );

            var regimeTributario = oportunidade.Cliente?.RegimeTributario ?? RegimeTributario.LucroPresumido;
            var (aliqPis, aliqCofins) = ObterAliquotasPorRegime(regimeTributario);

            if (!request.ContasSelecionadas.Any())
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Nenhuma conta selecionada para cálculo",
                    400
                );

            // Buscar plano de contas selecionado (apenas CodNatureza = '04' - despesas/custos)
            var planoContas = await _spedContabilI050Repository.GetByOportunidadeIdAsync(request.OportunidadeId);
            var contasFiltradas = planoContas
                .Where(c => request.ContasSelecionadas.Contains(c.CodigoCta) && c.CodNatureza == "04")
                .ToList();

            if (!contasFiltradas.Any())
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Nenhuma conta de natureza '04' (despesas/custos) foi selecionada",
                    400
                );

            // Buscar saldos (I155) das contas selecionadas
            var todosSaldos = await _spedContabilI155Repository.GetByOportunidadeIdAsync(request.OportunidadeId);
            var codigosSelecionados = contasFiltradas.Select(c => c.CodigoCta).ToList();
            var saldosFiltrados = todosSaldos
                .Where(s => codigosSelecionados.Contains(s.CodCta))
                .ToList();

            if (!saldosFiltrados.Any())
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Nenhum saldo (I155) encontrado para as contas selecionadas",
                    400
                );

            // Calcular PIS e COFINS para cada saldo usando I155
            var resultados = saldosFiltrados.Select(saldo =>
            {
                var conta = contasFiltradas.FirstOrDefault(c => c.CodigoCta == saldo.CodCta);
                var valorBase = saldo.ValorDebito ?? 0; // Usar valor de débito (despesas)
                var valorPis = valorBase * (aliqPis / 100) * request.Modalidade;
                var valorCofins = valorBase * (aliqCofins / 100) * request.Modalidade;

                return new InsumosResultado
                {
                    OportunidadeId = request.OportunidadeId,
                    DataApuracao = saldo.DataInicio,
                    DescricaoVerba = conta?.NomeCta ?? saldo.CodCta,
                    CodigoCta = saldo.CodCta,
                    ValorBase = valorBase,
                    ValorPis = valorPis,
                    ValorCofins = valorCofins,
                    ValorTotal = valorPis + valorCofins
                };
            }).ToList();

            // Salvar resultados no banco
            await _insumosResultadoRepository.SaveResultadosAsync(resultados, request.OportunidadeId);

            // Converter para DTOs
            var dtos = resultados.Select(r => new InsumosResultadoDto
            {
                DataApuracao = r.DataApuracao,
                DescricaoVerba = r.DescricaoVerba,
                CodigoCta = r.CodigoCta,
                ValorBase = r.ValorBase,
                ValorPis = r.ValorPis,
                ValorCofins = r.ValorCofins,
                ValorTotal = r.ValorTotal
            }).ToList();

            return new Response<List<InsumosResultadoDto>>(
                true,
                $"Cálculo realizado com sucesso. Modalidade: {(request.Modalidade == 1.0m ? "100%" : "70%")}",
                dtos,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<InsumosResultadoDto>>(
                false,
                $"Erro ao calcular insumos: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<List<InsumosResultadoDto>>> GetResultadoAsync(int oportunidadeId)
    {
        try
        {
            var resultados = await _insumosResultadoRepository.GetByOportunidadeIdAsync(oportunidadeId);

            if (!resultados.Any())
                return new Response<List<InsumosResultadoDto>>(
                    false,
                    "Nenhum resultado encontrado para esta Oportunidade",
                    404
                );

            var dtos = resultados.Select(r => new InsumosResultadoDto
            {
                DataApuracao = r.DataApuracao,
                DescricaoVerba = r.DescricaoVerba,
                CodigoCta = r.CodigoCta,
                ValorBase = r.ValorBase,
                ValorPis = r.ValorPis,
                ValorCofins = r.ValorCofins,
                ValorTotal = r.ValorTotal
            }).ToList();

            return new Response<List<InsumosResultadoDto>>(
                true,
                "Sucesso",
                dtos,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<InsumosResultadoDto>>(
                false,
                $"Erro ao buscar resultado: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<List<SpedContabilI050Dto>>> GetPlanoContasAsync(int oportunidadeId)
    {
        try
        {
            var planoContas = await _spedContabilI050Repository.GetByOportunidadeIdAsync(oportunidadeId);

            // Buscar saldos (I155) para calcular valor total por conta
            var saldos = await _spedContabilI155Repository.GetByOportunidadeIdAsync(oportunidadeId);
            var saldosPorConta = saldos
                .GroupBy(s => s.CodCta)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(s => s.ValorDebito ?? 0) // Soma dos débitos (despesas)
                );

            var dtos = planoContas.Select(p => new SpedContabilI050Dto
            {
                Id = p.Id,
                CodigoCta = p.CodigoCta,
                NomeCta = p.NomeCta,
                CodNatureza = p.CodNatureza,
                DataInicial = p.DataInicial,
                DataFinal = p.DataFinal,
                Status = p.Status,
                QtdI250 = p.QtdI250,
                QtdI250Selecionados = p.QtdI250Selecionados,
                ValorTotal = saldosPorConta.ContainsKey(p.CodigoCta) ? saldosPorConta[p.CodigoCta] : 0
            }).ToList();

            return new Response<List<SpedContabilI050Dto>>(
                true,
                "Sucesso",
                dtos,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<SpedContabilI050Dto>>(
                false,
                $"Erro ao buscar plano de contas: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<List<SpedContabilI250Dto>>> GetInsumosByContaAsync(int oportunidadeId, string codigoCta)
    {
        try
        {
            var insumos = await _spedContabilI250Repository.GetByCodigoCtaAsync(oportunidadeId, codigoCta);

            var dtos = insumos.Select(i => new SpedContabilI250Dto
            {
                Id = i.Id,
                CodigoCta = i.CodigoCta,
                DataApuracao = i.DataApuracao,
                Descricao = i.Descricao,
                Valor = i.Valor,
                Situacao = i.Situacao,
                IndicadorDC = i.IndicadorDC
            }).ToList();

            return new Response<List<SpedContabilI250Dto>>(
                true,
                "Sucesso",
                dtos,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<List<SpedContabilI250Dto>>(
                false,
                $"Erro ao buscar insumos: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<bool>> SelecionarI050Async(SelecionarI050RequestDto request)
    {
        try
        {
            await _spedContabilI050Repository.SelecionarAsync(request.OportunidadeId, request.I050Ids);

            return new Response<bool>(
                true,
                "Contas selecionadas com sucesso",
                true,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<bool>(
                false,
                $"Erro ao selecionar contas: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<bool>> SelecionarI250Async(SelecionarI250RequestDto request)
    {
        try
        {
            await _spedContabilI250Repository.SelecionarAsync(
                request.OportunidadeId,
                request.CodigoCta,
                request.I250Ids
            );

            return new Response<bool>(
                true,
                "Insumos selecionados com sucesso",
                true,
                200
            );
        }
        catch (Exception ex)
        {
            return new Response<bool>(
                false,
                $"Erro ao selecionar insumos: {ex.Message}",
                500
            );
        }
    }

    public async Task<Response<byte[]>> ExportToExcelAsync(int oportunidadeId)
    {
        try
        {
            var oportunidade = await _oportunidadeRepository.GetByIdAsync(oportunidadeId);
            if (oportunidade == null)
                return new Response<byte[]>(false, "Oportunidade não encontrada", null, 404);

            var resultados = await _insumosResultadoRepository.GetByOportunidadeIdAsync(oportunidadeId);
            if (!resultados.Any())
                return new Response<byte[]>(false, "Nenhum resultado encontrado para esta Oportunidade", null, 404);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Resultados de Insumos");

            // Configurar largura das colunas
            worksheet.Column(1).Width = 15;  // Data Apuração
            worksheet.Column(2).Width = 40;  // Descrição
            worksheet.Column(3).Width = 15;  // Código Conta
            worksheet.Column(4).Width = 18;  // Valor Base
            worksheet.Column(5).Width = 18;  // Valor PIS
            worksheet.Column(6).Width = 18;  // Valor COFINS
            worksheet.Column(7).Width = 18;  // Valor Total

            // Cabeçalho
            int currentRow = 1;
            worksheet.Cell(currentRow++, 1).Value = oportunidade.Cliente?.Nome ?? "Cliente";
            worksheet.Cell(currentRow++, 1).Value = $"CNPJ: {oportunidade.Cliente?.CNPJ ?? "N/A"}";
            worksheet.Cell(currentRow++, 1).Value = "Relatório de Créditos PIS/COFINS sobre Insumos";
            worksheet.Cell(currentRow++, 1).Value = $"Data: {DateTime.Now:dd/MM/yyyy}";

            // Linha de cabeçalho das colunas
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = "Data Apuração";
            worksheet.Cell(currentRow, 2).Value = "Descrição";
            worksheet.Cell(currentRow, 3).Value = "Código Conta";
            worksheet.Cell(currentRow, 4).Value = "Valor Base";
            worksheet.Cell(currentRow, 5).Value = "Valor PIS";
            worksheet.Cell(currentRow, 6).Value = "Valor COFINS";
            worksheet.Cell(currentRow, 7).Value = "Valor Total";

            // Estilizar cabeçalho
            var headerRange = worksheet.Range(currentRow, 1, currentRow, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            currentRow++;

            // Dados
            decimal totalBase = 0, totalPis = 0, totalCofins = 0, total = 0;

            foreach (var resultado in resultados.OrderBy(r => r.DataApuracao).ThenBy(r => r.CodigoCta))
            {
                worksheet.Cell(currentRow, 1).Value = resultado.DataApuracao?.ToString("MM/yyyy") ?? "";
                worksheet.Cell(currentRow, 2).Value = resultado.DescricaoVerba;
                worksheet.Cell(currentRow, 3).Value = resultado.CodigoCta;
                worksheet.Cell(currentRow, 4).Value = resultado.ValorBase ?? 0;
                worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 5).Value = resultado.ValorPis ?? 0;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 6).Value = resultado.ValorCofins ?? 0;
                worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 7).Value = resultado.ValorTotal ?? 0;
                worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "R$ #,##0.00";

                totalBase += resultado.ValorBase ?? 0;
                totalPis += resultado.ValorPis ?? 0;
                totalCofins += resultado.ValorCofins ?? 0;
                total += resultado.ValorTotal ?? 0;

                currentRow++;
            }

            // Linha de totais
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = "TOTAIS";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 4).Value = totalBase;
            worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 5).Value = totalPis;
            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 6).Value = totalCofins;
            worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 7).Value = total;
            worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 7).Style.Font.Bold = true;

            var totalRange = worksheet.Range(currentRow, 1, currentRow, 7);
            totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Salvar em memória
            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return new Response<byte[]>(true, "Success", fileBytes, 200);
        }
        catch (Exception ex)
        {
            return new Response<byte[]>(false, $"Erro ao exportar: {ex.Message}", null, 500);
        }
    }

    public async Task<Response<byte[]>> ExportToPdfAsync(int oportunidadeId)
    {
        try
        {
            // Configurar licença QuestPDF (Community License - gratuita)
            QuestPDF.Settings.License = LicenseType.Community;

            var oportunidade = await _oportunidadeRepository.GetByIdAsync(oportunidadeId);
            if (oportunidade == null)
                return new Response<byte[]>(false, "Oportunidade não encontrada", null, 404);

            var resultados = await _insumosResultadoRepository.GetByOportunidadeIdAsync(oportunidadeId);
            if (!resultados.Any())
                return new Response<byte[]>(false, "Nenhum resultado encontrado para esta Oportunidade", null, 404);

            // Ordenar resultados
            var resultadosOrdenados = resultados.OrderBy(r => r.DataApuracao).ThenBy(r => r.CodigoCta).ToList();

            // Calcular totais
            decimal totalBase = resultados.Sum(r => r.ValorBase ?? 0);
            decimal totalPis = resultados.Sum(r => r.ValorPis ?? 0);
            decimal totalCofins = resultados.Sum(r => r.ValorCofins ?? 0);
            decimal total = resultados.Sum(r => r.ValorTotal ?? 0);

            // Gerar PDF
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape()); // Paisagem para caber todas as colunas
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    // Cabeçalho
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text(oportunidade.Cliente?.Nome ?? "Cliente")
                            .FontSize(16).Bold().FontColor(Colors.Blue.Darken2);

                        column.Item().AlignCenter().Text($"CNPJ: {oportunidade.Cliente?.CNPJ ?? "N/A"}")
                            .FontSize(10);

                        column.Item().AlignCenter().Text("Relatório de Créditos PIS/COFINS sobre Insumos")
                            .FontSize(12).Bold();

                        column.Item().AlignCenter().Text($"Data: {DateTime.Now:dd/MM/yyyy}")
                            .FontSize(9);

                        column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Conteúdo
                    page.Content().PaddingTop(10).Table(table =>
                    {
                        // Definir colunas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);   // Data
                            columns.RelativeColumn(3);    // Descrição
                            columns.ConstantColumn(80);   // Código Conta
                            columns.ConstantColumn(90);   // Valor Base
                            columns.ConstantColumn(90);   // Valor PIS
                            columns.ConstantColumn(90);   // Valor COFINS
                            columns.ConstantColumn(90);   // Valor Total
                        });

                        // Cabeçalho da tabela
                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCellStyle).Text("Data");
                            header.Cell().Element(HeaderCellStyle).Text("Descrição");
                            header.Cell().Element(HeaderCellStyle).Text("Código Conta");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Valor Base");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Valor PIS");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Valor COFINS");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Valor Total");
                        });

                        // Dados
                        foreach (var resultado in resultadosOrdenados)
                        {
                            table.Cell().Element(DataCellStyle).Text(resultado.DataApuracao?.ToString("MM/yyyy") ?? "-");
                            table.Cell().Element(DataCellStyle).Text(resultado.DescricaoVerba ?? "");
                            table.Cell().Element(DataCellStyle).Text(resultado.CodigoCta ?? "");
                            table.Cell().Element(DataCellStyle).AlignRight().Text(FormatCurrency(resultado.ValorBase));
                            table.Cell().Element(DataCellStyle).AlignRight().Text(FormatCurrency(resultado.ValorPis));
                            table.Cell().Element(DataCellStyle).AlignRight().Text(FormatCurrency(resultado.ValorCofins));
                            table.Cell().Element(DataCellStyle).AlignRight().Text(FormatCurrency(resultado.ValorTotal));
                        }

                        // Linha de totais
                        table.Cell().Element(TotalCellStyle).Text("TOTAIS").Bold();
                        table.Cell().Element(TotalCellStyle).Text("");
                        table.Cell().Element(TotalCellStyle).Text("");
                        table.Cell().Element(TotalCellStyle).AlignRight().Text(FormatCurrency(totalBase)).Bold();
                        table.Cell().Element(TotalCellStyle).AlignRight().Text(FormatCurrency(totalPis)).Bold();
                        table.Cell().Element(TotalCellStyle).AlignRight().Text(FormatCurrency(totalCofins)).Bold();
                        table.Cell().Element(TotalCellStyle).AlignRight().Text(FormatCurrency(total)).Bold();
                    });

                    // Rodapé
                    page.Footer().AlignRight().DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Medium))
                        .Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();

            return new Response<byte[]>(true, "Success", pdfBytes, 200);
        }
        catch (Exception ex)
        {
            return new Response<byte[]>(false, $"Erro ao exportar PDF: {ex.Message}", null, 500);
        }
    }

    // Métodos auxiliares para estilização do PDF
    private static IContainer HeaderCellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.Blue.Lighten3)
            .Padding(5)
            .AlignMiddle();
    }

    private static IContainer DataCellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5)
            .AlignMiddle();
    }

    private static IContainer TotalCellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Medium)
            .Background(Colors.Grey.Lighten3)
            .Padding(5)
            .AlignMiddle();
    }

    private static string FormatCurrency(decimal? value)
    {
        if (!value.HasValue) return "R$ 0,00";
        return value.Value.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
    }
}
