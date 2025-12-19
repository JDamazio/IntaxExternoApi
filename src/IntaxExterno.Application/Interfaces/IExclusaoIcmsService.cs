using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IExclusaoIcmsService
{
    Task<Response<List<ExclusaoIcmsResultadoDto>>> CalcularExclusaoAsync(CalcularExclusaoRequestDto request);
    Task<Response<List<ExclusaoIcmsResultadoDto>>> GetResultadoAsync(int oportunidadeId);
}
