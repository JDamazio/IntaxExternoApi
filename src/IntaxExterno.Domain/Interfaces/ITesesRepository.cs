using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface ITesesRepository
{
    Task<Teses> CreateAsync(Teses teses, string createdById);
    Task<IEnumerable<Teses>> GetAllAsync();
    Task<Teses?> GetByIdAsync(int id);
    Task<Teses> UpdateAsync(Teses teses, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
