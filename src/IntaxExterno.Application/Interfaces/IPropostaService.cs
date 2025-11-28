using IntaxExterno.Application.DTOs.Proposta;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IPropostaService
{
    Task<Response<PropostaPostDto>> CreateAsync(PropostaPostDto propostaPostDto, string createdById);
    Task<Response<IEnumerable<PropostaGetDto>>> GetAllAsync();
    Task<Response<PropostaGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<PropostaPutDto>> UpdateAsync(PropostaPutDto propostaPutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int propostaId, string deletedById);
}
