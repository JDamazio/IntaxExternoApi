using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IInsumosResultadoRepository
{
    Task<List<InsumosResultado>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task SaveResultadosAsync(List<InsumosResultado> resultados, int oportunidadeId);
    Task DeleteByOportunidadeIdAsync(int oportunidadeId);
}
