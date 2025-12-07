using Microsoft.EntityFrameworkCore;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Context;

namespace IntaxExterno.Infra.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente> CreateAsync(Cliente cliente, string createdById)
    {
        cliente.Create(createdById);

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> GetByCNPJAsync(string cnpj)
    {
        return await _context.Clientes
            .Where(c => c.CNPJ == cnpj && c.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task<Cliente?> UpdateAsync(Cliente cliente, string updatedById)
    {
        var existingCliente = await _context.Clientes.FindAsync(cliente.Id);
        if (existingCliente == null)
            return null;

        // Atualizar apenas os campos específicos da entidade
        existingCliente.Nome = cliente.Nome;
        existingCliente.Telefone = cliente.Telefone;
        existingCliente.Email = cliente.Email;
        existingCliente.EmailResponsavel = cliente.EmailResponsavel;
        existingCliente.CNPJ = cliente.CNPJ;

        // Atualizar campos de auditoria
        existingCliente.Update(updatedById);

        await _context.SaveChangesAsync();
        return existingCliente;
    }

    public async Task<bool> DeleteAsync(int id, string deletedById)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return false;

        cliente.Delete(deletedById);

        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
}
