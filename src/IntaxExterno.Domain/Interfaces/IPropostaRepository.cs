using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IPropostaRepository
{
    Task<Proposta> CreateAsync(Proposta proposta, string createdById);
    Task<IEnumerable<Proposta>> GetAllAsync();
    Task<Proposta?> GetByIdAsync(int id);
    Task<Proposta> UpdateAsync(Proposta proposta, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
