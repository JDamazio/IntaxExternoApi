using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IExclusaoIcmsResultadoRepository
{
    Task<List<ExclusaoIcmsResultado>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task SaveResultadosAsync(List<ExclusaoIcmsResultado> resultados, int oportunidadeId);
    Task DeleteByOportunidadeIdAsync(int oportunidadeId);
}
