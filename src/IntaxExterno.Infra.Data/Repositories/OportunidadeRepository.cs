using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class OportunidadeRepository : IOportunidadeRepository
{
    private readonly ApplicationDbContext _context;

    public OportunidadeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Oportunidade> CreateAsync(Oportunidade oportunidade, string createdById)
    {
        oportunidade.Create(createdById);
        _context.Oportunidades.Add(oportunidade);
        await _context.SaveChangesAsync();
        return oportunidade;
    }

    public async Task<IEnumerable<Oportunidade>> GetAllAsync()
    {
        return await _context.Oportunidades
            .Include(o => o.Cliente)
            .Include(o => o.Parceiro)
            .Include(o => o.OportunidadeTeses)
            .ToListAsync();
    }

    public async Task<Oportunidade?> GetByIdAsync(int id)
    {
        return await _context.Oportunidades
            .Include(o => o.Cliente)
            .Include(o => o.Parceiro)
            .Include(o => o.OportunidadeTeses)
                .ThenInclude(ot => ot.Teses)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Oportunidade?> UpdateAsync(Oportunidade oportunidade, string updatedById)
    {
        var existing = await _context.Oportunidades.FindAsync(oportunidade.Id);
        if (existing == null) return null;

        existing.ClienteId = oportunidade.ClienteId;
        existing.ParceiroId = oportunidade.ParceiroId;
        existing.Descricao = oportunidade.Descricao;
        existing.DataInicio = oportunidade.DataInicio;
        existing.DataFechamento = oportunidade.DataFechamento;
        existing.Status = oportunidade.Status;
        existing.Update(updatedById);

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var oportunidade = await _context.Oportunidades.FindAsync(id);
        if (oportunidade == null) return false;

        oportunidade.Delete(deletedById);
        _context.Oportunidades.Update(oportunidade);
        await _context.SaveChangesAsync();
        return true;
    }
}
