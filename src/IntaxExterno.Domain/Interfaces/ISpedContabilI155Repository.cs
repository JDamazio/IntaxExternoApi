using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ISpedContabilI155Repository
{
    Task<IEnumerable<SpedContabilI155>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<IEnumerable<SpedContabilI155>> GetByCodigoCtaAsync(int oportunidadeId, string codigoCta);
    Task SaveAsync(IEnumerable<SpedContabilI155> registros, int oportunidadeId);
    Task DeleteByOportunidadeIdAsync(int oportunidadeId);
}
