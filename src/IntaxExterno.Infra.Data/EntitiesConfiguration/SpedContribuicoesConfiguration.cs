using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class SpedContribuicoesConfiguration : IEntityTypeConfiguration<SpedContribuicoes>
{
    public void Configure(EntityTypeBuilder<SpedContribuicoes> builder)
    {
        builder.Property(s => s.CodFiscal).HasMaxLength(10).IsRequired();
        builder.Property(s => s.CodSitPis).HasMaxLength(10).IsRequired();
        builder.Property(s => s.Regime).HasMaxLength(10).IsRequired();

        builder.Property(s => s.AliqPis).HasColumnType("decimal(18,4)");
        builder.Property(s => s.AliqCofins).HasColumnType("decimal(18,4)");
        builder.Property(s => s.ValorIcms).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorPis).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorCofins).HasColumnType("decimal(18,2)");
        builder.Property(s => s.ValorPisCofins).HasColumnType("decimal(18,2)");

        // Índices
        builder.HasIndex(s => s.OportunidadeId).HasDatabaseName("IX_SpedContribuicoes_OportunidadeId");
        builder.HasIndex(s => s.DataInicial).HasDatabaseName("IX_SpedContribuicoes_DataInicial");
        builder.HasIndex(s => s.IsActive).HasDatabaseName("IX_SpedContribuicoes_IsActive");

        // Relacionamento
        builder.HasOne(s => s.Oportunidade)
            .WithMany(o => o.SpedContribuicoes)
            .HasForeignKey(s => s.OportunidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
