using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class InsumosResultadoRepository : IInsumosResultadoRepository
{
    private readonly ApplicationDbContext _context;

    public InsumosResultadoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InsumosResultado>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.InsumosResultados
            .Where(r => r.OportunidadeId == oportunidadeId && r.IsActive)
            .OrderBy(r => r.DataApuracao)
            .ThenBy(r => r.CodigoCta)
            .ToListAsync();
    }

    public async Task SaveResultadosAsync(List<InsumosResultado> resultados, int oportunidadeId)
    {
        // Remove resultados anteriores
        await DeleteByOportunidadeIdAsync(oportunidadeId);

        // Adiciona novos resultados
        foreach (var resultado in resultados)
        {
            resultado.Create("system");
            _context.InsumosResultados.Add(resultado);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByOportunidadeIdAsync(int oportunidadeId)
    {
        var resultadosExistentes = await _context.InsumosResultados
            .Where(r => r.OportunidadeId == oportunidadeId && r.IsActive)
            .ToListAsync();

        foreach (var resultado in resultadosExistentes)
        {
            resultado.Delete("system");
        }

        await _context.SaveChangesAsync();
    }
}
