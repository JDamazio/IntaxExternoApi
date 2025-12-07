using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class RelatorioDeCreditoPerseRepository : IRelatorioDeCreditoPerseRepository
{
    private readonly ApplicationDbContext _context;

    public RelatorioDeCreditoPerseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RelatorioDeCreditoPerse> CreateAsync(RelatorioDeCreditoPerse relatorioDeCreditoPerse, string createdById)
    {
        relatorioDeCreditoPerse.Create(createdById);
        _context.RelatoriosDeCreditoPerse.Add(relatorioDeCreditoPerse);
        await _context.SaveChangesAsync();
        return relatorioDeCreditoPerse;
    }

    public async Task<IEnumerable<RelatorioDeCreditoPerse>> GetAllAsync()
    {
        return await _context.RelatoriosDeCreditoPerse
            .Include(r => r.Cliente)
            .Include(r => r.Itens.Where(i => i.IsActive))
            .Where(r => r.IsActive)
            .ToListAsync();
    }

    public async Task<RelatorioDeCreditoPerse?> GetByIdAsync(int id)
    {
        return await _context.RelatoriosDeCreditoPerse
            .Include(r => r.Cliente)
            .Include(r => r.Itens.Where(i => i.IsActive))
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
    }

    public async Task<RelatorioDeCreditoPerse?> UpdateAsync(RelatorioDeCreditoPerse relatorioDeCreditoPerse, string updatedById)
    {
        var existingRelatorio = await _context.RelatoriosDeCreditoPerse
            .Include(r => r.Cliente)
            .FirstOrDefaultAsync(r => r.Id == relatorioDeCreditoPerse.Id);

        if (existingRelatorio == null)
            return null;

        // Atualizar apenas os campos específicos da entidade
        existingRelatorio.DataEmissao = relatorioDeCreditoPerse.DataEmissao;
        existingRelatorio.TotalIRPJ = relatorioDeCreditoPerse.TotalIRPJ;
        existingRelatorio.TotalCSLL = relatorioDeCreditoPerse.TotalCSLL;
        existingRelatorio.TotalPIS = relatorioDeCreditoPerse.TotalPIS;
        existingRelatorio.TotalCOFINS = relatorioDeCreditoPerse.TotalCOFINS;
        existingRelatorio.Total = relatorioDeCreditoPerse.Total;
        existingRelatorio.Saldo = relatorioDeCreditoPerse.Saldo;
        existingRelatorio.ClienteId = relatorioDeCreditoPerse.ClienteId;

        // Atualizar campos de auditoria
        existingRelatorio.Update(updatedById);

        await _context.SaveChangesAsync();

        // Recarregar os itens atualizados
        await _context.Entry(existingRelatorio)
            .Collection(r => r.Itens)
            .Query()
            .Where(i => i.IsActive)
            .LoadAsync();

        return existingRelatorio;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var relatorioDeCreditoPerse = await _context.RelatoriosDeCreditoPerse.FindAsync(id);
        if (relatorioDeCreditoPerse == null)
            return false;

        relatorioDeCreditoPerse.Delete(deletedById);

        _context.RelatoriosDeCreditoPerse.Update(relatorioDeCreditoPerse);
        await _context.SaveChangesAsync();
        return true;
    }
}
