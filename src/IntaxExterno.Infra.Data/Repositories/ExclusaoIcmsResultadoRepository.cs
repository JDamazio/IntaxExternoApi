using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class ExclusaoIcmsResultadoRepository : IExclusaoIcmsResultadoRepository
{
    private readonly ApplicationDbContext _context;

    public ExclusaoIcmsResultadoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExclusaoIcmsResultado>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        return await _context.ExclusaoIcmsResultados
            .Where(r => r.OportunidadeId == oportunidadeId && r.IsActive)
            .OrderBy(r => r.DataInicial)
            .ToListAsync();
    }

    public async Task SaveResultadosAsync(List<ExclusaoIcmsResultado> resultados, int oportunidadeId)
    {
        // Remove resultados anteriores
        await DeleteByOportunidadeIdAsync(oportunidadeId);

        // Adiciona novos resultados
        foreach (var resultado in resultados)
        {
            resultado.Create("system");
            _context.ExclusaoIcmsResultados.Add(resultado);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteByOportunidadeIdAsync(int oportunidadeId)
    {
        var resultadosExistentes = await _context.ExclusaoIcmsResultados
            .Where(r => r.OportunidadeId == oportunidadeId && r.IsActive)
            .ToListAsync();

        foreach (var resultado in resultadosExistentes)
        {
            resultado.Delete("system");
        }

        await _context.SaveChangesAsync();
    }
}
