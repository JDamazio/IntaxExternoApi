using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Domain.Interfaces;

public interface IPropostaTesesRepository
{
    Task<PropostaTeses> CreateAsync(PropostaTeses propostaTeses, string createdById);
    Task<IEnumerable<PropostaTeses>> GetByPropostaIdAsync(int propostaId);
    Task DeleteByPropostaIdAsync(int propostaId, string deletedById);
}
