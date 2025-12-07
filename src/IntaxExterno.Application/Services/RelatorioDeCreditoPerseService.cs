using AutoMapper;
using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using ClosedXML.Excel;

namespace IntaxExterno.Application.Services;

public class RelatorioDeCreditoPerseService : IRelatorioDeCreditoPerseService
{
    private readonly IRelatorioDeCreditoPerseRepository _relatorioRepository;
    private readonly IItemRelatorioDeCreditoPerseRepository _itemRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public RelatorioDeCreditoPerseService(
        IRelatorioDeCreditoPerseRepository relatorioRepository,
        IItemRelatorioDeCreditoPerseRepository itemRepository,
        IClienteRepository clienteRepository,
        IMapper mapper)
    {
        _relatorioRepository = relatorioRepository;
        _itemRepository = itemRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<Response<RelatorioDeCreditoPersePostDto>> CreateAsync(RelatorioDeCreditoPersePostDto relatorioDeCreditoPersePostDto, string createdById)
    {
        RelatorioDeCreditoPerse? relatorio = _mapper.Map<RelatorioDeCreditoPerse>(relatorioDeCreditoPersePostDto);
        RelatorioDeCreditoPerse? createdRelatorio = await _relatorioRepository.CreateAsync(relatorio, createdById);
        if (createdRelatorio == null)
            return new Response<RelatorioDeCreditoPersePostDto>(false, "Failed to create relatorio de credito perse", null, 500);

        // Criar registros na tabela ItemRelatorioDeCreditoPerse
        if (relatorioDeCreditoPersePostDto.Itens != null && relatorioDeCreditoPersePostDto.Itens.Any())
        {
            foreach (var itemDto in relatorioDeCreditoPersePostDto.Itens)
            {
                var item = _mapper.Map<ItemRelatorioDeCreditoPerse>(itemDto);
                item.RelatorioDeCreditoPerseId = createdRelatorio.Id;
                await _itemRepository.CreateAsync(item, createdById);
            }
        }

        return new Response<RelatorioDeCreditoPersePostDto>(true, "Success", _mapper.Map<RelatorioDeCreditoPersePostDto>(createdRelatorio), 201);
    }

    public async Task<Response<IEnumerable<RelatorioDeCreditoPerseGetDto>>> GetAllAsync()
    {
        IEnumerable<RelatorioDeCreditoPerse> relatorios = await _relatorioRepository.GetAllAsync();

        if (!relatorios.Any())
            return new Response<IEnumerable<RelatorioDeCreditoPerseGetDto>>(false, "No relatorios de credito perse found", null, 404);

        return new Response<IEnumerable<RelatorioDeCreditoPerseGetDto>>(true, "Success", _mapper.Map<IEnumerable<RelatorioDeCreditoPerseGetDto>>(relatorios), 200);
    }

    public async Task<Response<RelatorioDeCreditoPerseGetDetailsDto?>> GetByIdAsync(int id)
    {
        RelatorioDeCreditoPerse? relatorio = await _relatorioRepository.GetByIdAsync(id);
        if (relatorio == null)
        {
            return new Response<RelatorioDeCreditoPerseGetDetailsDto?>(false, "Relatorio de credito perse not found", 404);
        }
        return new Response<RelatorioDeCreditoPerseGetDetailsDto?>(true, "Success", _mapper.Map<RelatorioDeCreditoPerseGetDetailsDto>(relatorio), 200);
    }

    public async Task<Response<RelatorioDeCreditoPersePutDto>> UpdateAsync(RelatorioDeCreditoPersePutDto relatorioDeCreditoPersePutDto, string updatedById)
    {
        RelatorioDeCreditoPerse? relatorio = _mapper.Map<RelatorioDeCreditoPerse>(relatorioDeCreditoPersePutDto);
        RelatorioDeCreditoPerse? updatedRelatorio = await _relatorioRepository.UpdateAsync(relatorio, updatedById);
        if (updatedRelatorio == null)
        {
            return new Response<RelatorioDeCreditoPersePutDto>(false, "Relatorio de credito perse not found", 404);
        }

        // Atualizar registros na tabela ItemRelatorioDeCreditoPerse
        var existingItens = await _itemRepository.GetByRelatorioIdAsync(updatedRelatorio.Id);
        var existingItensIds = existingItens.Select(i => i.Id).ToList();

        if (relatorioDeCreditoPersePutDto.Itens != null && relatorioDeCreditoPersePutDto.Itens.Any())
        {
            var dtoItensIds = relatorioDeCreditoPersePutDto.Itens
                .Where(i => i.Id > 0)
                .Select(i => i.Id)
                .ToList();

            // Deletar itens que foram removidos (existem no banco mas não no DTO)
            foreach (var existingItem in existingItens)
            {
                if (!dtoItensIds.Contains(existingItem.Id))
                {
                    existingItem.Delete(updatedById);
                }
            }

            // Atualizar ou criar itens
            foreach (var itemDto in relatorioDeCreditoPersePutDto.Itens)
            {
                var item = _mapper.Map<ItemRelatorioDeCreditoPerse>(itemDto);
                item.RelatorioDeCreditoPerseId = updatedRelatorio.Id;

                if (itemDto.Id > 0 && existingItensIds.Contains(itemDto.Id))
                {
                    // Item existe - atualizar
                    await _itemRepository.UpdateAsync(item, updatedById);
                }
                else
                {
                    // Item novo - criar
                    await _itemRepository.CreateAsync(item, updatedById);
                }
            }
        }
        else
        {
            // Se não há itens no DTO, deletar todos os existentes
            await _itemRepository.DeleteByRelatorioIdAsync(updatedRelatorio.Id, updatedById);
        }

        return new Response<RelatorioDeCreditoPersePutDto>(true, "Success", _mapper.Map<RelatorioDeCreditoPersePutDto>(updatedRelatorio), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int relatorioDeCreditoPerseId, string deletedById)
    {
        RelatorioDeCreditoPerse? relatorio = await _relatorioRepository.GetByIdAsync(id);
        if (relatorio == null)
        {
            return new Response<bool>(false, "Relatorio de credito perse not found", 404);
        }
        if (relatorio.Id != relatorioDeCreditoPerseId)
        {
            return new Response<bool>(false, "You are not allowed to delete this relatorio de credito perse", 403);
        }
        bool result = await _relatorioRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }

    public async Task<Response<RelatorioImportResultDto>> ImportFromExcelAsync(IFormFile file, string createdById, DateTime dataEmissao)
    {
        var result = new RelatorioImportResultDto();

        try
        {
            if (file == null || file.Length == 0)
                return new Response<RelatorioImportResultDto>(false, "Arquivo inválido", result, 400);

            var allowedExtensions = new[] { ".xlsx", ".xls", ".csv" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return new Response<RelatorioImportResultDto>(false, "Formato de arquivo não suportado. Use .xlsx, .xls ou .csv", result, 400);

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            // Se for CSV, processar como texto
            if (extension == ".csv")
            {
                return await ProcessCsvImport(stream, createdById, result, dataEmissao);
            }

            // Se for Excel, usar ClosedXML
            IXLWorksheet worksheet;
            try
            {
                var workbook = new XLWorkbook(stream);
                worksheet = workbook.Worksheet(1);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Erro ao abrir arquivo Excel: {ex.Message}");
                return new Response<RelatorioImportResultDto>(false, "Arquivo Excel inválido ou corrompido", result, 400);
            }

            // Ler cabeçalho do relatório
            var cnpjLine = worksheet.Cell(2, 1).GetString();
            var cnpj = cnpjLine.Replace("CNPJ:", "").Replace("CNPJ", "").Trim().Replace(".", "").Replace("/", "").Replace("-", "");

            // Buscar cliente pelo CNPJ
            var cliente = await _clienteRepository.GetByCNPJAsync(cnpj);
            if (cliente == null)
            {
                result.Errors.Add($"Cliente com CNPJ {cnpj} não encontrado no sistema.");
                return new Response<RelatorioImportResultDto>(false, "Cliente não encontrado", result, 404);
            }

            // Procurar linha com "PERÍODO DE CRÉDITO" (cabeçalho dos totais)
            // Não deve conter "DO" para não pegar "PERÍODO DO CRÉDITO" que é o cabeçalho dos itens
            int headerRow = 0;
            for (int row = 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
            {
                var cellValue = worksheet.Cell(row, 1).GetString().Trim().ToUpper();
                // Verifica se contém "PERÍODO" mas NÃO contém "PERÍODO DO"
                if ((cellValue.Contains("PERÍODO DE CRÉDITO") || cellValue.Contains("PERIODO DE CREDITO"))
                    && !cellValue.Contains("PERÍODO DO") && !cellValue.Contains("PERIODO DO"))
                {
                    headerRow = row;
                    break;
                }
            }

            if (headerRow == 0)
            {
                result.Errors.Add("Não foi possível encontrar o cabeçalho 'PERÍODO DE CRÉDITO' no arquivo.");
                return new Response<RelatorioImportResultDto>(false, "Formato de arquivo inválido", result, 400);
            }

            // A linha de totais é a próxima após o cabeçalho
            int totalRow = headerRow + 1;

            // Nota: dataEmissao vem como parâmetro da API (informado pelo usuário)
            // Não precisamos ler do arquivo, pois é apenas para auditoria/identificação

            var totalIRPJ = ParseDecimal(worksheet.Cell(totalRow, 2).GetString());
            var totalCSLL = ParseDecimal(worksheet.Cell(totalRow, 3).GetString());
            var totalPIS = ParseDecimal(worksheet.Cell(totalRow, 4).GetString());
            var totalCOFINS = ParseDecimal(worksheet.Cell(totalRow, 5).GetString());
            var total = ParseDecimal(worksheet.Cell(totalRow, 6).GetString());

            // Procurar linha do cabeçalho dos itens (PERÍODO DO CRÉDITO)
            int itemHeaderRow = 0;
            for (int row = totalRow; row <= worksheet.LastRowUsed().RowNumber(); row++)
            {
                var cellValue = worksheet.Cell(row, 1).GetString().Trim().ToUpper();
                if (cellValue.Contains("PERÍODO DO CRÉDITO") || cellValue.Contains("PERIODO DO CREDITO"))
                {
                    itemHeaderRow = row;
                    break;
                }
            }

            // Ler itens
            var itens = new List<ItemRelatorioDeCreditoPerse>();
            if (itemHeaderRow > 0)
            {
                for (int row = itemHeaderRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    var periodoStr = worksheet.Cell(row, 1).GetString().Trim().ToUpper();

                    // Parar ao encontrar TOTAL ou SALDO
                    if (periodoStr == "TOTAL" || periodoStr == "SALDO" || string.IsNullOrWhiteSpace(periodoStr))
                        break;

                    // Parse do período (ex: "IRPJ 01/2022")
                    var parts = periodoStr.Split(' ');
                    if (parts.Length < 2)
                        continue;

                    var tipoTributo = parts[0]; // IRPJ, CSLL, PIS, COFINS
                    var mesAno = parts[1].Split('/'); // 01/2022

                    if (mesAno.Length != 2)
                        continue;

                    if (!int.TryParse(mesAno[0], out int mesPeriodoItem) || !int.TryParse(mesAno[1], out int anoItem))
                        continue;

                    // Criar data de emissão a partir do mês e ano parseados (UTC, meio-dia)
                    DateTime dataEmissaoItem = new DateTime(anoItem, mesPeriodoItem, 1, 12, 0, 0, DateTimeKind.Utc);

                    var numPedidoStr = worksheet.Cell(row, 2).GetString();
                    int? numPedido = null;
                    if (int.TryParse(numPedidoStr, out int pedido))
                        numPedido = pedido;

                    var item = new ItemRelatorioDeCreditoPerse
                    {
                        TipoTributo = tipoTributo,
                        DataEmissao = dataEmissaoItem,
                        NumPedido = numPedido,
                        TotalSolicitado = ParseDecimal(worksheet.Cell(row, 3).GetString()),
                        CorrecaoMonetaria = ParseDecimal(worksheet.Cell(row, 4).GetString()),
                        TotalRecebido = ParseDecimal(worksheet.Cell(row, 5).GetString()),
                        Observacao = worksheet.Cell(row, 6).GetString()
                    };

                    itens.Add(item);
                }
            }

            // Procurar linha do SALDO
            decimal saldo = 0;
            for (int row = 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
            {
                if (worksheet.Cell(row, 1).GetString().Trim().ToUpper() == "SALDO")
                {
                    saldo = ParseDecimal(worksheet.Cell(row, 3).GetString());
                    break;
                }
            }

            // Criar relatório
            var relatorio = new RelatorioDeCreditoPerse
            {
                ClienteId = cliente.Id,
                DataEmissao = dataEmissao,
                TotalIRPJ = totalIRPJ,
                TotalCSLL = totalCSLL,
                TotalPIS = totalPIS,
                TotalCOFINS = totalCOFINS,
                Total = total,
                Saldo = saldo
            };

            var createdRelatorio = await _relatorioRepository.CreateAsync(relatorio, createdById);

            // Criar itens
            foreach (var item in itens)
            {
                item.RelatorioDeCreditoPerseId = createdRelatorio.Id;
                await _itemRepository.CreateAsync(item, createdById);
            }

            result.Success = true;
            result.Message = "Relatório importado com sucesso";
            result.RelatoriosImportados = 1;
            result.ItensImportados = itens.Count;

            return new Response<RelatorioImportResultDto>(true, "Importação realizada com sucesso", result, 201);
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Erro ao processar arquivo: {ex.Message}");
            result.Success = false;
            return new Response<RelatorioImportResultDto>(false, "Erro ao importar arquivo", result, 500);
        }
    }

    public async Task<Response<byte[]>> ExportToExcelAsync(int id)
    {
        try
        {
            var relatorio = await _relatorioRepository.GetByIdAsync(id);
            if (relatorio == null)
                return new Response<byte[]>(false, "Relatório não encontrado", null, 404);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Créditos");

            // Configurar largura das colunas
            worksheet.Column(1).Width = 25;
            worksheet.Column(2).Width = 15;
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 40;

            // Cabeçalho
            int currentRow = 1;
            worksheet.Cell(currentRow++, 1).Value = relatorio.Cliente.Nome;
            worksheet.Cell(currentRow++, 1).Value = $"CNPJ: {relatorio.Cliente.CNPJ}";
            worksheet.Cell(currentRow++, 1).Value = "Relatório de Créditos PERSE - (Restituição)";

            // Linha de totais
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = "PERÍODO DE CRÉDITO";
            worksheet.Cell(currentRow, 2).Value = "IRPJ";
            worksheet.Cell(currentRow, 3).Value = "CSLL";
            worksheet.Cell(currentRow, 4).Value = "PIS";
            worksheet.Cell(currentRow, 5).Value = "COFINS";
            worksheet.Cell(currentRow, 6).Value = "TOTAL";

            // Estilizar cabeçalho
            var headerRange = worksheet.Range(currentRow, 1, currentRow, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            currentRow++;
            worksheet.Cell(currentRow, 1).Value = relatorio.DataEmissao.ToString("MM/yyyy");
            worksheet.Cell(currentRow, 2).Value = relatorio.TotalIRPJ;
            worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 3).Value = relatorio.TotalCSLL;
            worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 4).Value = relatorio.TotalPIS;
            worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 5).Value = relatorio.TotalCOFINS;
            worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 6).Value = relatorio.Total;
            worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "R$ #,##0.00";

            // Relatório discriminado
            currentRow += 2;
            worksheet.Cell(currentRow++, 1).Value = "RELATÓRIO DISCRIMINADO DE RESTITUIÇÃO";

            worksheet.Cell(currentRow, 1).Value = "PERÍODO DO CRÉDITO";
            worksheet.Cell(currentRow, 2).Value = "Nº DO PEDIDO";
            worksheet.Cell(currentRow, 3).Value = "TOTAL SOLICITADO";
            worksheet.Cell(currentRow, 4).Value = "CORREÇÃO MONETARIA";
            worksheet.Cell(currentRow, 5).Value = "TOTAL RECEBIDO";
            worksheet.Cell(currentRow, 6).Value = "OBSERVAÇÃO";

            var detailHeaderRange = worksheet.Range(currentRow, 1, currentRow, 6);
            detailHeaderRange.Style.Font.Bold = true;
            detailHeaderRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            currentRow++;

            // Itens
            var itensOrdenados = relatorio.Itens.OrderBy(i => i.TipoTributo).ThenBy(i => i.DataEmissao).ToList();

            foreach (var item in itensOrdenados)
            {
                worksheet.Cell(currentRow, 1).Value = $"{item.TipoTributo} {item.DataEmissao:MM/yyyy}";
                worksheet.Cell(currentRow, 2).Value = item.NumPedido?.ToString() ?? "";
                worksheet.Cell(currentRow, 3).Value = item.TotalSolicitado;
                worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 4).Value = item.CorrecaoMonetaria ?? 0;
                worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 5).Value = item.TotalRecebido ?? 0;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Cell(currentRow, 6).Value = item.Observacao ?? "";
                currentRow++;
            }

            // Total e Saldo
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = "TOTAL";
            worksheet.Cell(currentRow, 3).Value = relatorio.Total;
            worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;

            currentRow++;
            worksheet.Cell(currentRow, 1).Value = "SALDO";
            worksheet.Cell(currentRow, 3).Value = relatorio.Saldo;
            worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;

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

    private async Task<Response<RelatorioImportResultDto>> ProcessCsvImport(MemoryStream stream, string createdById, RelatorioImportResultDto result, DateTime dataEmissao)
    {
        try
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var lines = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }

            if (lines.Count < 5)
            {
                result.Errors.Add("Arquivo CSV não contém dados suficientes");
                return new Response<RelatorioImportResultDto>(false, "Arquivo CSV inválido", result, 400);
            }

            // Detectar delimitador (vírgula, ponto-e-vírgula ou tab)
            var delimiter = DetectCsvDelimiter(lines[0]);

            // Ler CNPJ (linha 2)
            var cnpjParts = lines[1].Split(delimiter);
            var cnpj = cnpjParts[0].Replace("CNPJ:", "").Replace("CNPJ", "").Trim()
                .Replace(".", "").Replace("/", "").Replace("-", "");

            // Buscar cliente
            var cliente = await _clienteRepository.GetByCNPJAsync(cnpj);
            if (cliente == null)
            {
                result.Errors.Add($"Cliente com CNPJ {cnpj} não encontrado no sistema.");
                return new Response<RelatorioImportResultDto>(false, "Cliente não encontrado", result, 404);
            }

            // Procurar linha com totais (primeira linha que começa com um ano)
            int totalRowIndex = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(delimiter);
                if (parts.Length > 0 && int.TryParse(parts[0].Trim(), out int year) && year >= 2000 && year <= 2100)
                {
                    totalRowIndex = i;
                    break;
                }
            }

            if (totalRowIndex == -1)
            {
                result.Errors.Add("Não foi possível encontrar a linha de totais no CSV");
                return new Response<RelatorioImportResultDto>(false, "Formato de CSV inválido", result, 400);
            }

            var totalParts = lines[totalRowIndex].Split(delimiter);
            // Nota: dataEmissao vem como parâmetro da API (informado pelo usuário)
            var totalIRPJ = totalParts.Length > 1 ? ParseDecimal(totalParts[1]) : 0;
            var totalCSLL = totalParts.Length > 2 ? ParseDecimal(totalParts[2]) : 0;
            var totalPIS = totalParts.Length > 3 ? ParseDecimal(totalParts[3]) : 0;
            var totalCOFINS = totalParts.Length > 4 ? ParseDecimal(totalParts[4]) : 0;
            var total = totalParts.Length > 5 ? ParseDecimal(totalParts[5]) : 0;

            // Procurar seção de itens (após "PERÍODO DO CRÉDITO")
            int itemHeaderIndex = -1;
            for (int i = totalRowIndex; i < lines.Count; i++)
            {
                if (lines[i].ToUpper().Contains("PERÍODO DO CRÉDITO") || lines[i].ToUpper().Contains("PERIODO DO CREDITO"))
                {
                    itemHeaderIndex = i;
                    break;
                }
            }

            // Ler itens
            var itens = new List<ItemRelatorioDeCreditoPerse>();
            if (itemHeaderIndex != -1)
            {
                for (int i = itemHeaderIndex + 1; i < lines.Count; i++)
                {
                    var itemParts = lines[i].Split(delimiter);
                    if (itemParts.Length == 0 || string.IsNullOrWhiteSpace(itemParts[0]))
                        continue;

                    var periodoStr = itemParts[0].Trim().ToUpper();

                    // Parar em TOTAL ou SALDO
                    if (periodoStr == "TOTAL" || periodoStr == "SALDO")
                        break;

                    // Parse período (ex: "IRPJ 01/2022")
                    var parts = periodoStr.Split(' ');
                    if (parts.Length < 2)
                        continue;

                    var tipoTributo = parts[0];
                    var mesAno = parts[1].Split('/');

                    if (mesAno.Length != 2)
                        continue;

                    if (!int.TryParse(mesAno[0], out int mesPeriodoItem) || !int.TryParse(mesAno[1], out int anoItem))
                        continue;

                    // Criar data de emissão a partir do mês e ano parseados (UTC, meio-dia)
                    DateTime dataEmissaoItem = new DateTime(anoItem, mesPeriodoItem, 1, 12, 0, 0, DateTimeKind.Utc);

                    int? numPedido = null;
                    if (itemParts.Length > 1 && int.TryParse(itemParts[1].Trim(), out int pedido))
                        numPedido = pedido;

                    var item = new ItemRelatorioDeCreditoPerse
                    {
                        TipoTributo = tipoTributo,
                        DataEmissao = dataEmissaoItem,
                        NumPedido = numPedido,
                        TotalSolicitado = itemParts.Length > 2 ? ParseDecimal(itemParts[2]) : 0,
                        CorrecaoMonetaria = itemParts.Length > 3 ? ParseDecimal(itemParts[3]) : 0,
                        TotalRecebido = itemParts.Length > 4 ? ParseDecimal(itemParts[4]) : 0,
                        Observacao = itemParts.Length > 5 ? itemParts[5].Trim() : null
                    };

                    itens.Add(item);
                }
            }

            // Procurar SALDO
            decimal saldo = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].ToUpper().StartsWith("SALDO"))
                {
                    var saldoParts = lines[i].Split(delimiter);
                    if (saldoParts.Length > 2)
                        saldo = ParseDecimal(saldoParts[2]);
                    break;
                }
            }

            // Criar relatório
            var relatorio = new RelatorioDeCreditoPerse
            {
                ClienteId = cliente.Id,
                DataEmissao = dataEmissao,
                TotalIRPJ = totalIRPJ,
                TotalCSLL = totalCSLL,
                TotalPIS = totalPIS,
                TotalCOFINS = totalCOFINS,
                Total = total,
                Saldo = saldo
            };

            var createdRelatorio = await _relatorioRepository.CreateAsync(relatorio, createdById);

            // Criar itens
            foreach (var item in itens)
            {
                item.RelatorioDeCreditoPerseId = createdRelatorio.Id;
                await _itemRepository.CreateAsync(item, createdById);
            }

            result.Success = true;
            result.Message = "Relatório CSV importado com sucesso";
            result.RelatoriosImportados = 1;
            result.ItensImportados = itens.Count;

            return new Response<RelatorioImportResultDto>(true, "Importação realizada com sucesso", result, 201);
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Erro ao processar CSV: {ex.Message}");
            result.Success = false;
            return new Response<RelatorioImportResultDto>(false, "Erro ao importar CSV", result, 500);
        }
    }

    private char DetectCsvDelimiter(string firstLine)
    {
        // Contar ocorrências de cada delimitador possível
        var commaCount = firstLine.Count(c => c == ',');
        var semicolonCount = firstLine.Count(c => c == ';');
        var tabCount = firstLine.Count(c => c == '\t');

        // Retornar o delimitador mais comum
        if (semicolonCount > commaCount && semicolonCount > tabCount)
            return ';';
        if (tabCount > commaCount && tabCount > semicolonCount)
            return '\t';

        return ','; // padrão
    }

    private decimal ParseDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        // Remove R$, espaços e converte vírgula para ponto
        value = value.Replace("R$", "").Replace(" ", "").Trim();

        // Se tiver ponto e vírgula, assume formato brasileiro (1.234,56)
        if (value.Contains(".") && value.Contains(","))
        {
            value = value.Replace(".", "").Replace(",", ".");
        }
        // Se tiver só vírgula, assume formato brasileiro (1234,56)
        else if (value.Contains(","))
        {
            value = value.Replace(",", ".");
        }

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            return result;

        return 0;
    }
}
