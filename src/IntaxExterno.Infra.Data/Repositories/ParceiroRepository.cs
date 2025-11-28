using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class ParceiroRepository : IParceiroRepository
{
    private readonly ApplicationDbContext _context;

    public ParceiroRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Parceiro> CreateAsync(Parceiro parceiro, string createdById)
    {
        parceiro.Create(createdById);

        _context.Parceiros.Add(parceiro);
        await _context.SaveChangesAsync();
        return parceiro;
    }

    public async Task<IEnumerable<Parceiro>> GetAllAsync()
    {
        return await _context.Parceiros.ToListAsync();
    }

    public async Task<Parceiro?> GetByIdAsync(int id)
    {
        return await _context.Parceiros.FindAsync(id);
    }

    public async Task<Parceiro> UpdateAsync(Parceiro parceiro, string updatedById)
    {
        parceiro.Update(updatedById);

        _context.Parceiros.Update(parceiro);
        await _context.SaveChangesAsync();
        return parceiro;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var parceiro = await _context.Parceiros.FindAsync(id);
        if (parceiro == null)
            return false;

        parceiro.Delete(deletedById);

        _context.Parceiros.Update(parceiro);
        await _context.SaveChangesAsync();
        return true;
    }
}
