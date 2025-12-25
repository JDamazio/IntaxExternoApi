using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ISpedContabilI050Repository
{
    Task<List<SpedContabilI050>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<List<SpedContabilI050>> GetSelecionadosByOportunidadeIdAsync(int oportunidadeId);
    Task SaveAsync(List<SpedContabilI050> planoContas, int oportunidadeId);
    Task SelecionarAsync(int oportunidadeId, List<int> ids);
    Task DeleteByOportunidadeIdAsync(int oportunidadeId);
}
