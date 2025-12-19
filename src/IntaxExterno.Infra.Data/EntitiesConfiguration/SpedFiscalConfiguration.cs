using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class SpedFiscalConfiguration : IEntityTypeConfiguration<SpedFiscal>
{
    public void Configure(EntityTypeBuilder<SpedFiscal> builder)
    {
        builder.Property(s => s.Cfop).HasMaxLength(10).IsRequired();
        builder.Property(s => s.CstIcms).HasMaxLength(10).IsRequired();

        builder.Property(s => s.AliqPis).HasColumnType("decimal(18,4)");
        builder.Property(s => s.AliqCofins).HasColumnType("decimal(18,4)");
        builder.Property(s => s.ValorIcms).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorPis).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorCofins).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorPisCofins).HasColumnType("decimal(18,2)");

        // Índices
        builder.HasIndex(s => s.OportunidadeId).HasDatabaseName("IX_SpedFiscal_OportunidadeId");
        builder.HasIndex(s => s.DataInicial).HasDatabaseName("IX_SpedFiscal_DataInicial");
        builder.HasIndex(s => s.IsActive).HasDatabaseName("IX_SpedFiscal_IsActive");

        // Relacionamento
        builder.HasOne(s => s.Oportunidade)
            .WithMany(o => o.SpedFiscais)
            .HasForeignKey(s => s.OportunidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
