using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class SpedContabilI050Repository : ISpedContabilI050Repository
{
    private readonly ApplicationDbContext _context;

    public SpedContabilI050Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpedContabilI050>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.SpedContabilI050
            .Where(p => p.OportunidadeId == oportunidadeId && p.IsActive)
            .OrderBy(p => p.CodigoCta)
            .ToListAsync();
    }

    public async Task<List<SpedContabilI050>> GetSelecionadosByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.SpedContabilI050
            .Where(p => p.OportunidadeId == oportunidadeId && p.IsActive && p.Status == 2) // Status 2 = Selecionado
            .OrderBy(p => p.CodigoCta)
            .ToListAsync();
    }

    public async Task SaveAsync(List<SpedContabilI050> planoContas, int oportunidadeId)
    {
        // Remove plano de contas anterior
        await DeleteByOportunidadeIdAsync(oportunidadeId);

        // Adiciona novo plano de contas
        foreach (var conta in planoContas)
        {
            conta.Create("system");
            _context.SpedContabilI050.Add(conta);
        }

        await _context.SaveChangesAsync();
    }

    public async Task SelecionarAsync(int oportunidadeId, List<int> ids)
    {
        var todasContas = await _context.SpedContabilI050
            .Where(p => p.OportunidadeId == oportunidadeId && p.IsActive)
            .ToListAsync();

        foreach (var conta in todasContas)
        {
            // Status: 0=Pendente, 2=Selecionado
            conta.Status = ids.Contains(conta.Id) ? 2 : 0;
            conta.Update("system");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByOportunidadeIdAsync(int oportunidadeId)
    {
        var contasExistentes = await _context.SpedContabilI050
            .Where(p => p.OportunidadeId == oportunidadeId && p.IsActive)
            .ToListAsync();

        foreach (var conta in contasExistentes)
        {
            conta.Delete("system");
        }

        await _context.SaveChangesAsync();
    }
}
