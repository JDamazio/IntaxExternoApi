using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class ItemRelatorioDeCreditoPerseRepository : IItemRelatorioDeCreditoPerseRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRelatorioDeCreditoPerseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ItemRelatorioDeCreditoPerse> CreateAsync(ItemRelatorioDeCreditoPerse itemRelatorioDeCreditoPerse, string createdById)
    {
        itemRelatorioDeCreditoPerse.Create(createdById);

        _context.Set<ItemRelatorioDeCreditoPerse>().Add(itemRelatorioDeCreditoPerse);
        await _context.SaveChangesAsync();
        return itemRelatorioDeCreditoPerse;
    }

    public async Task<ItemRelatorioDeCreditoPerse?> UpdateAsync(ItemRelatorioDeCreditoPerse itemRelatorioDeCreditoPerse, string updatedById)
    {
        var existingItem = await _context.Set<ItemRelatorioDeCreditoPerse>().FindAsync(itemRelatorioDeCreditoPerse.Id);
        if (existingItem == null)
            return null;

        // Atualizar apenas os campos específicos da entidade
        existingItem.TipoTributo = itemRelatorioDeCreditoPerse.TipoTributo;
        existingItem.DataEmissao = itemRelatorioDeCreditoPerse.DataEmissao;
        existingItem.NumPedido = itemRelatorioDeCreditoPerse.NumPedido;
        existingItem.TotalSolicitado = itemRelatorioDeCreditoPerse.TotalSolicitado;
        existingItem.CorrecaoMonetaria = itemRelatorioDeCreditoPerse.CorrecaoMonetaria;
        existingItem.TotalRecebido = itemRelatorioDeCreditoPerse.TotalRecebido;
        existingItem.Observacao = itemRelatorioDeCreditoPerse.Observacao;

        // Atualizar campos de auditoria
        existingItem.Update(updatedById);

        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task<IEnumerable<ItemRelatorioDeCreditoPerse>> GetByRelatorioIdAsync(int relatorioId)
    {
        return await _context.Set<ItemRelatorioDeCreditoPerse>()
            .Where(i => i.RelatorioDeCreditoPerseId == relatorioId && i.IsActive)
            .ToListAsync();
    }

    public async Task DeleteByRelatorioIdAsync(int relatorioId, string deletedById)
    {
        var itens = await _context.Set<ItemRelatorioDeCreditoPerse>()
            .Where(i => i.RelatorioDeCreditoPerseId == relatorioId && i.IsActive)
            .ToListAsync();

        foreach (var item in itens)
        {
            item.Delete(deletedById);
            _context.Set<ItemRelatorioDeCreditoPerse>().Update(item);
        }

        await _context.SaveChangesAsync();
    }
}
