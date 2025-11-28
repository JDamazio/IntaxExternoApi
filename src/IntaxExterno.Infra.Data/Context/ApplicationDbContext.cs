using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntaxExterno.Infra.Data.Identity;
using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Infra.Data.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // DbSets
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Proposta> Propostas { get; set; }
    public DbSet<PropostaTeses> PropostaTeses { get; set; }
    public DbSet<Teses> Teses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
