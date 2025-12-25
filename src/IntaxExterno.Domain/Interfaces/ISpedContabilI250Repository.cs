using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ISpedContabilI250Repository
{
    Task<List<SpedContabilI250>> GetByOportunidadeIdAsync(int oportunidadeId);
    Task<List<SpedContabilI250>> GetByCodigoCtaAsync(int oportunidadeId, string codigoCta);
    Task<List<SpedContabilI250>> GetSelecionadosByOportunidadeIdAsync(int oportunidadeId);
    Task SaveAsync(List<SpedContabilI250> insumos, int oportunidadeId);
    Task SelecionarAsync(int oportunidadeId, string codigoCta, List<int> ids);
    Task DeleteByOportunidadeIdAsync(int oportunidadeId);
}
