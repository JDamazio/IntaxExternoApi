using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class SpedContabilI250Repository : ISpedContabilI250Repository
{
    private readonly ApplicationDbContext _context;

    public SpedContabilI250Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpedContabilI250>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.SpedContabilI250
            .Where(i => i.OportunidadeId == oportunidadeId && i.IsActive)
            .OrderBy(i => i.CodigoCta)
            .ThenBy(i => i.DataApuracao)
            .ToListAsync();
    }

    public async Task<List<SpedContabilI250>> GetByCodigoCtaAsync(int oportunidadeId, string codigoCta)
    {
        return await _context.SpedContabilI250
            .Where(i => i.OportunidadeId == oportunidadeId && i.CodigoCta == codigoCta && i.IsActive)
            .OrderBy(i => i.DataApuracao)
            .ToListAsync();
    }

    public async Task<List<SpedContabilI250>> GetSelecionadosByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.SpedContabilI250
            .Where(i => i.OportunidadeId == oportunidadeId && i.IsActive && i.Situacao == 0) // Situacao 0 = Ativo
            .OrderBy(i => i.CodigoCta)
            .ThenBy(i => i.DataApuracao)
            .ToListAsync();
    }

    public async Task SaveAsync(List<SpedContabilI250> insumos, int oportunidadeId)
    {
        // Remove insumos anteriores
        await DeleteByOportunidadeIdAsync(oportunidadeId);

        // Adiciona novos insumos
        foreach (var insumo in insumos)
        {
            insumo.Create("system");
            _context.SpedContabilI250.Add(insumo);
        }

        await _context.SaveChangesAsync();
    }

    public async Task SelecionarAsync(int oportunidadeId, string codigoCta, List<int> ids)
    {
        var todosInsumos = await _context.SpedContabilI250
            .Where(i => i.OportunidadeId == oportunidadeId && i.CodigoCta == codigoCta && i.IsActive)
            .ToListAsync();

        foreach (var insumo in todosInsumos)
        {
            // Situacao: 0=Ativo (incluído), 1=Removido (excluído)
            insumo.Situacao = ids.Contains(insumo.Id) ? 0 : 1;
            insumo.Update("system");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByOportunidadeIdAsync(int oportunidadeId)
    {
        var insumosExistentes = await _context.SpedContabilI250
            .Where(i => i.OportunidadeId == oportunidadeId && i.IsActive)
            .ToListAsync();

        foreach (var insumo in insumosExistentes)
        {
            insumo.Delete("system");
        }

        await _context.SaveChangesAsync();
    }
}
