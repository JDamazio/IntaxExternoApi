using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IParceiroRepository
{
    Task<Parceiro> CreateAsync(Parceiro parceiro, string createdById);
    Task<IEnumerable<Parceiro>> GetAllAsync();
    Task<Parceiro?> GetByIdAsync(int id);
    Task<Parceiro?> UpdateAsync(Parceiro parceiro, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
