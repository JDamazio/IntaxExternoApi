using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using IntaxExterno.Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace IntaxExterno.Application.Interfaces;

public interface IRelatorioDeCreditoPerseService
{
    Task<Response<RelatorioDeCreditoPersePostDto>> CreateAsync(RelatorioDeCreditoPersePostDto relatorioDeCreditoPersePostDto, string createdById);
    Task<Response<IEnumerable<RelatorioDeCreditoPerseGetDto>>> GetAllAsync();
    Task<Response<RelatorioDeCreditoPerseGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<RelatorioDeCreditoPersePutDto>> UpdateAsync(RelatorioDeCreditoPersePutDto relatorioDeCreditoPersePutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int relatorioDeCreditoPerseId, string deletedById);
    Task<Response<RelatorioImportResultDto>> ImportFromExcelAsync(IFormFile file, string createdById);
    Task<Response<byte[]>> ExportToExcelAsync(int id);
}
