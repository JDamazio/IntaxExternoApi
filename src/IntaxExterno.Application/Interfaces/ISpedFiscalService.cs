using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface ISpedFiscalService
{
    Task<Response<List<SpedFiscalGetDto>>> CreateManyAsync(List<SpedFiscalPostDto> dtos, string createdById);
    Task<Response<List<SpedFiscalGetDto>>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<Response<bool>> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById);
}
