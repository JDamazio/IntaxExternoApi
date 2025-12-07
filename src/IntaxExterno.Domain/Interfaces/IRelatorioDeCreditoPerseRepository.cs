using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IRelatorioDeCreditoPerseRepository
{
    Task<RelatorioDeCreditoPerse> CreateAsync(RelatorioDeCreditoPerse relatorioDeCreditoPerse, string createdById);
    Task<IEnumerable<RelatorioDeCreditoPerse>> GetAllAsync();
    Task<RelatorioDeCreditoPerse?> GetByIdAsync(int id);
    Task<RelatorioDeCreditoPerse?> UpdateAsync(RelatorioDeCreditoPerse relatorioDeCreditoPerse, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
