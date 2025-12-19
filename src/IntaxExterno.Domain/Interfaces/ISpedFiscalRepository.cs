using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ISpedFiscalRepository
{
    Task<SpedFiscal> CreateAsync(SpedFiscal sped, string createdById);
    Task<IEnumerable<SpedFiscal>> CreateManyAsync(IEnumerable<SpedFiscal> speds, string createdById);
    Task<IEnumerable<SpedFiscal>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<SpedFiscal?> GetByIdAsync(int id);
    Task<bool> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById);
}
