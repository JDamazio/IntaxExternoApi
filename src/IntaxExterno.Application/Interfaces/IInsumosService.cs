using IntaxExterno.Application.DTOs.Insumos;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IInsumosService
{
    Task<Response<bool>> ProcessarSpedContabilAsync(int oportunidadeId, Stream arquivo);
    Task<Response<List<InsumosResultadoDto>>> CalcularInsumosAsync(CalcularInsumosRequestDto request);
    Task<Response<List<InsumosResultadoDto>>> GetResultadoAsync(int oportunidadeId);
    Task<Response<List<SpedContabilI050Dto>>> GetPlanoContasAsync(int oportunidadeId);
    Task<Response<List<SpedContabilI250Dto>>> GetInsumosByContaAsync(int oportunidadeId, string codigoCta);
    Task<Response<bool>> SelecionarI050Async(SelecionarI050RequestDto request);
    Task<Response<bool>> SelecionarI250Async(SelecionarI250RequestDto request);
    Task<Response<byte[]>> ExportToExcelAsync(int oportunidadeId);
    Task<Response<byte[]>> ExportToPdfAsync(int oportunidadeId);
}
