using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface ITesesService
{
    Task<Response<TesesPostDto>> CreateAsync(TesesPostDto tesesPostDto, string createdById);
    Task<Response<IEnumerable<TesesGetDto>>> GetAllAsync();
    Task<Response<TesesGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<TesesPutDto>> UpdateAsync(TesesPutDto tesesPutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int tesesId, string deletedById);
}
