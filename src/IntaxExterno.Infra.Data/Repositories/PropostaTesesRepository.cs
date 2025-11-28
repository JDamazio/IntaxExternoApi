using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class PropostaTesesRepository : IPropostaTesesRepository
{
    private readonly ApplicationDbContext _context;

    public PropostaTesesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PropostaTeses> CreateAsync(PropostaTeses propostaTeses, string createdById)
    {
        propostaTeses.Create(createdById);

        _context.Set<PropostaTeses>().Add(propostaTeses);
        await _context.SaveChangesAsync();
        return propostaTeses;
    }

    public async Task<IEnumerable<PropostaTeses>> GetByPropostaIdAsync(int propostaId)
    {
        return await _context.Set<PropostaTeses>()
            .Where(pt => pt.PropostaId == propostaId && pt.IsActive)
            .Include(pt => pt.Teses)
            .ToListAsync();
    }

    public async Task DeleteByPropostaIdAsync(int propostaId, string deletedById)
    {
        var propostaTeses = await _context.Set<PropostaTeses>()
            .Where(pt => pt.PropostaId == propostaId && pt.IsActive)
            .ToListAsync();

        foreach (var pt in propostaTeses)
        {
            pt.Delete(deletedById);
            _context.Set<PropostaTeses>().Update(pt);
        }

        await _context.SaveChangesAsync();
    }
}
