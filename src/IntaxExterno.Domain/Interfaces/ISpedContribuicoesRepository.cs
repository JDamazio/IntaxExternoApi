using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ISpedContribuicoesRepository
{
    Task<SpedContribuicoes> CreateAsync(SpedContribuicoes sped, string createdById);
    Task<IEnumerable<SpedContribuicoes>> CreateManyAsync(IEnumerable<SpedContribuicoes> speds, string createdById);
    Task<IEnumerable<SpedContribuicoes>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<SpedContribuicoes?> GetByIdAsync(int id);
    Task<bool> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById);
}
