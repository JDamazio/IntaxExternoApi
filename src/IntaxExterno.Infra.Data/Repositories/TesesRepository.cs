using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class TesesRepository : ITesesRepository
{
    private readonly ApplicationDbContext _context;

    public TesesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Teses> CreateAsync(Teses teses, string createdById)
    {
        teses.Create(createdById);

        _context.Teses.Add(teses);
        await _context.SaveChangesAsync();
        return teses;
    }

    public async Task<IEnumerable<Teses>> GetAllAsync()
    {
        return await _context.Teses.ToListAsync();
    }

    public async Task<Teses?> GetByIdAsync(int id)
    {
        return await _context.Teses.FindAsync(id);
    }

    public async Task<Teses?> UpdateAsync(Teses teses, string updatedById)
    {
        var existingTeses = await _context.Teses.FindAsync(teses.Id);
        if (existingTeses == null)
            return null;

        // Atualizar apenas os campos específicos da entidade
        existingTeses.Nome = teses.Nome;
        existingTeses.Descrição = teses.Descrição;

        // Atualizar campos de auditoria
        existingTeses.Update(updatedById);

        await _context.SaveChangesAsync();
        return existingTeses;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var teses = await _context.Teses.FindAsync(id);
        if (teses == null)
            return false;

        teses.Delete(deletedById);

        _context.Teses.Update(teses);
        await _context.SaveChangesAsync();
        return true;
    }
}
