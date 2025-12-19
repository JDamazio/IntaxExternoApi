using IntaxExterno.Application.DTOs.Oportunidade;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IOportunidadeService
{
    Task<Response<OportunidadePostDto>> CreateAsync(OportunidadePostDto oportunidadePostDto, string createdById);
    Task<Response<IEnumerable<OportunidadeGetDto>>> GetAllAsync();
    Task<Response<OportunidadeGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<OportunidadePutDto>> UpdateAsync(OportunidadePutDto oportunidadePutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int oportunidadeId, string deletedById);
}
