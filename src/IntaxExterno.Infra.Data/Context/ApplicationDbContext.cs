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
    public DbSet<RelatorioDeCreditoPerse> RelatoriosDeCreditoPerse { get; set; }
    public DbSet<ItemRelatorioDeCreditoPerse> ItensRelatorioDeCreditoPerse { get; set; }
    public DbSet<Oportunidade> Oportunidades { get; set; }
    public DbSet<OportunidadeTeses> OportunidadeTeses { get; set; }
    public DbSet<ExclusaoIcmsResultado> ExclusaoIcmsResultados { get; set; }
    public DbSet<SpedContribuicoes> SpedContribuicoes { get; set; }
    public DbSet<SpedFiscal> SpedFiscais { get; set; }
    public DbSet<InsumosResultado> InsumosResultados { get; set; }
    public DbSet<SpedContabilI050> SpedContabilI050 { get; set; }
    public DbSet<SpedContabilI155> SpedContabilI155 { get; set; }
    public DbSet<SpedContabilI250> SpedContabilI250 { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
