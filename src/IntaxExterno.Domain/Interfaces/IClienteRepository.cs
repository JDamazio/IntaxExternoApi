using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente> CreateAsync(Cliente cliente, string createdById);
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task<Cliente> UpdateAsync(Cliente cliente, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
