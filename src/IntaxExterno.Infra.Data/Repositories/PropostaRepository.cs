using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class PropostaRepository : IPropostaRepository
{
    private readonly ApplicationDbContext _context;

    public PropostaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Proposta> CreateAsync(Proposta proposta, string createdById)
    {
        proposta.Create(createdById);
        _context.Propostas.Add(proposta);
        await _context.SaveChangesAsync();
        return proposta;
    }

    public async Task<IEnumerable<Proposta>> GetAllAsync()
    {
        return await _context.Propostas
            .Include(p => p.Cliente)
            .Include(p => p.Parceiro)
            .Include(p => p.PropostaTeses)
                .ThenInclude(pt => pt.Teses)
            .ToListAsync();
    }

    public async Task<Proposta?> GetByIdAsync(int id)
    {
        return await _context.Propostas
            .Include(p => p.Cliente)
            .Include(p => p.Parceiro)
            .Include(p => p.PropostaTeses)
                .ThenInclude(pt => pt.Teses)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Proposta> UpdateAsync(Proposta proposta, string updatedById)
    {
        proposta.Update(updatedById);

        _context.Propostas.Update(proposta);
        await _context.SaveChangesAsync();
        return proposta;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var proposta = await _context.Propostas.FindAsync(id);
        if (proposta == null)
            return false;

        proposta.Delete(deletedById);

        _context.Propostas.Update(proposta);
        await _context.SaveChangesAsync();
        return true;
    }
}
