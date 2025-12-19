using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IOportunidadeRepository
{
    Task<Oportunidade> CreateAsync(Oportunidade oportunidade, string createdById);
    Task<IEnumerable<Oportunidade>> GetAllAsync();
    Task<Oportunidade?> GetByIdAsync(int id);
    Task<Oportunidade?> UpdateAsync(Oportunidade oportunidade, string updatedById);
    Task<bool> DeleteAsync(int id, string deletedById);
}
