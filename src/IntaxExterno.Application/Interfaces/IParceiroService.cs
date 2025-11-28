using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IParceiroService
{
    Task<Response<ParceiroPostDto>> CreateAsync(ParceiroPostDto parceiroPostDto, string createdById);
    Task<Response<IEnumerable<ParceiroGetDto>>> GetAllAsync();
    Task<Response<ParceiroGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<ParceiroPutDto>> UpdateAsync(ParceiroPutDto parceiroPutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int parceiroId, string deletedById);
}
