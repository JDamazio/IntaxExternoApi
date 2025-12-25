using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class SpedContabilI155Configuration : IEntityTypeConfiguration<SpedContabilI155>
{
    public void Configure(EntityTypeBuilder<SpedContabilI155> builder)
    {
        builder.ToTable("SpedContabilI155");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CodCta)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(x => x.CodCcus)
            .HasMaxLength(60);

        builder.Property(x => x.ValorDebito)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ValorCredito)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IndicadorSituacao)
            .IsRequired()
            .HasMaxLength(1);

        // Relacionamento com Oportunidade
        builder.HasOne(x => x.Oportunidade)
            .WithMany()
            .HasForeignKey(x => x.OportunidadeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices para performance
        builder.HasIndex(x => x.OportunidadeId);
        builder.HasIndex(x => new { x.OportunidadeId, x.CodCta });
        builder.HasIndex(x => new { x.OportunidadeId, x.IndicadorSituacao });
        builder.HasIndex(x => new { x.OportunidadeId, x.DataInicio, x.DataFim });
    }
}
