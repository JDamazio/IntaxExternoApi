using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class SpedContabilI250Configuration : IEntityTypeConfiguration<SpedContabilI250>
{
    public void Configure(EntityTypeBuilder<SpedContabilI250> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UID)
            .HasMaxLength(36)
            .IsRequired();

        builder.HasIndex(e => e.UID)
            .IsUnique()
            .HasDatabaseName("IX_SpedContabilI250_UID");

        builder.Property(e => e.OportunidadeId)
            .IsRequired();

        builder.Property(e => e.CodigoCta)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.DataApuracao)
            .IsRequired(false);

        builder.Property(e => e.Descricao)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(e => e.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(e => e.Situacao)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.IndicadorDC)
            .HasMaxLength(1)
            .IsRequired(false);

        builder.Property(e => e.Created)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(e => e.Updated)
            .IsRequired(false);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(e => e.Deleted)
            .IsRequired(false);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_SpedContabilI250_IsActive");

        builder.HasIndex(e => new { e.OportunidadeId, e.IsActive })
            .HasDatabaseName("IX_SpedContabilI250_OportunidadeId_IsActive");

        builder.HasIndex(e => new { e.OportunidadeId, e.CodigoCta, e.Situacao })
            .HasDatabaseName("IX_SpedContabilI250_OportunidadeId_CodigoCta_Situacao");

        builder.HasIndex(e => new { e.OportunidadeId, e.DataApuracao })
            .HasDatabaseName("IX_SpedContabilI250_OportunidadeId_DataApuracao");

        // Relacionamento com Oportunidade
        builder.HasOne(e => e.Oportunidade)
            .WithMany()
            .HasForeignKey(e => e.OportunidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
