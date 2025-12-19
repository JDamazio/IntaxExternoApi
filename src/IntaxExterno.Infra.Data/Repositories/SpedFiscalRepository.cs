using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IntaxExterno.Infra.Data.Repositories;

public class SpedFiscalRepository : ISpedFiscalRepository
{
    private readonly ApplicationDbContext _context;

    public SpedFiscalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpedFiscal> CreateAsync(SpedFiscal sped, string createdById)
    {
        sped.Create(createdById);
        _context.Set<SpedFiscal>().Add(sped);
        await _context.SaveChangesAsync();
        return sped;
    }

    public async Task<IEnumerable<SpedFiscal>> CreateManyAsync(IEnumerable<SpedFiscal> speds, string createdById)
    {
        foreach (var sped in speds)
        {
            sped.Create(createdById);
        }

        _context.Set<SpedFiscal>().AddRange(speds);
        await _context.SaveChangesAsync();
        return speds;
    }

    public async Task<IEnumerable<SpedFiscal>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.Set<SpedFiscal>()
            .Where(s => s.OportunidadeId == oportunidadeId && s.IsActive)
            .OrderBy(s => s.DataInicial)
            .ToListAsync();
    }

    public async Task<SpedFiscal?> GetByIdAsync(int id)
    {
        return await _context.Set<SpedFiscal>()
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<bool> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById)
    {
        var speds = await _context.Set<SpedFiscal>()
            .Where(s => s.OportunidadeId == oportunidadeId && s.IsActive)
            .ToListAsync();

        if (!speds.Any())
            return false;

        foreach (var sped in speds)
        {
            sped.Delete(deletedById);
        }

        _context.Set<SpedFiscal>().UpdateRange(speds);
        await _context.SaveChangesAsync();
        return true;
    }
}
