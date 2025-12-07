using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IItemRelatorioDeCreditoPerseRepository
{
    Task<ItemRelatorioDeCreditoPerse> CreateAsync(ItemRelatorioDeCreditoPerse itemRelatorioDeCreditoPerse, string createdById);
    Task<IEnumerable<ItemRelatorioDeCreditoPerse>> GetByRelatorioIdAsync(int relatorioId);
    Task DeleteByRelatorioIdAsync(int relatorioId, string deletedById);
}
