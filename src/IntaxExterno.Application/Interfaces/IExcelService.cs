using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using Microsoft.AspNetCore.Http;

namespace IntaxExterno.Application.Interfaces;

public interface IExcelService
{
    Task<RelatorioImportResultDto> ImportRelatorioFromExcelAsync(IFormFile file, string createdById);
    Task<byte[]> ExportRelatorioToExcelAsync(int relatorioId);
}
