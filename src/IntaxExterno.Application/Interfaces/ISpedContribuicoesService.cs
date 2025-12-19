using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface ISpedContribuicoesService
{
    Task<Response<List<SpedContribuicoesGetDto>>> CreateManyAsync(List<SpedContribuicoesPostDto> dtos, string createdById);
    Task<Response<List<SpedContribuicoesGetDto>>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<Response<bool>> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById);
}
