using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IntaxExterno.Infra.Data.Repositories;

public class SpedContribuicoesRepository : ISpedContribuicoesRepository
{
    private readonly ApplicationDbContext _context;

    public SpedContribuicoesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpedContribuicoes> CreateAsync(SpedContribuicoes sped, string createdById)
    {
        sped.Create(createdById);
        _context.Set<SpedContribuicoes>().Add(sped);
        await _context.SaveChangesAsync();
        return sped;
    }

    public async Task<IEnumerable<SpedContribuicoes>> CreateManyAsync(IEnumerable<SpedContribuicoes> speds, string createdById)
    {
        foreach (var sped in speds)
        {
            sped.Create(createdById);
        }

        _context.Set<SpedContribuicoes>().AddRange(speds);
        await _context.SaveChangesAsync();
        return speds;
    }

    public async Task<IEnumerable<SpedContribuicoes>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.Set<SpedContribuicoes>()
            .Where(s => s.OportunidadeId == oportunidadeId && s.IsActive)
            .OrderBy(s => s.DataInicial)
            .ToListAsync();
    }

    public async Task<SpedContribuicoes?> GetByIdAsync(int id)
    {
        return await _context.Set<SpedContribuicoes>()
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<bool> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById)
    {
        var speds = await _context.Set<SpedContribuicoes>()
            .Where(s => s.OportunidadeId == oportunidadeId && s.IsActive)
            .ToListAsync();

        if (!speds.Any())
            return false;

        foreach (var sped in speds)
        {
            sped.Delete(deletedById);
        }

        _context.Set<SpedContribuicoes>().UpdateRange(speds);
        await _context.SaveChangesAsync();
        return true;
    }
}
