using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IntaxExterno.Infra.Data.Repositories;

public class SpedContabilI155Repository : ISpedContabilI155Repository
{
    private readonly ApplicationDbContext _context;

    public SpedContabilI155Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SpedContabilI155>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.SpedContabilI155
            .AsNoTracking()
            .Where(x => x.OportunidadeId == oportunidadeId && x.IsActive)
            .OrderBy(x => x.CodCta)
            .ThenBy(x => x.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<SpedContabilI155>> GetByCodigoCtaAsync(int oportunidadeId, string codigoCta)
    {
        return await _context.SpedContabilI155
            .AsNoTracking()
            .Where(x => x.OportunidadeId == oportunidadeId
                && x.CodCta == codigoCta
                && x.IsActive)
            .OrderBy(x => x.DataInicio)
            .ToListAsync();
    }

    public async Task SaveAsync(IEnumerable<SpedContabilI155> registros, int oportunidadeId)
    {
        // Performance: Delete antigos em uma única operação
        await _context.SpedContabilI155
            .Where(x => x.OportunidadeId == oportunidadeId)
            .ExecuteDeleteAsync();

        // Performance: Bulk insert com AddRange
        if (registros.Any())
        {
            // Popular UID e campos de auditoria para cada registro
            foreach (var registro in registros)
            {
                registro.Create("system");
            }

            await _context.SpedContabilI155.AddRangeAsync(registros);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByOportunidadeIdAsync(int oportunidadeId)
    {
        await _context.SpedContabilI155
            .Where(x => x.OportunidadeId == oportunidadeId)
            .ExecuteDeleteAsync();
    }
}
