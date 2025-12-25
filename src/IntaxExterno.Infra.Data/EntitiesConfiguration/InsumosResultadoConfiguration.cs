using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class InsumosResultadoConfiguration : IEntityTypeConfiguration<InsumosResultado>
{
    public void Configure(EntityTypeBuilder<InsumosResultado> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UID)
            .HasMaxLength(36)
            .IsRequired();

        builder.HasIndex(e => e.UID)
            .IsUnique()
            .HasDatabaseName("IX_InsumosResultado_UID");

        builder.Property(e => e.OportunidadeId)
            .IsRequired();

        builder.Property(e => e.DataApuracao)
            .IsRequired(false);

        builder.Property(e => e.DescricaoVerba)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.CodigoCta)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.ValorBase)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(e => e.ValorPis)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(e => e.ValorCofins)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(e => e.ValorTotal)
            .HasColumnType("decimal(18,2)")
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
            .HasDatabaseName("IX_InsumosResultado_IsActive");

        builder.HasIndex(e => new { e.OportunidadeId, e.IsActive })
            .HasDatabaseName("IX_InsumosResultado_OportunidadeId_IsActive");

        builder.HasIndex(e => new { e.CodigoCta, e.OportunidadeId })
            .HasDatabaseName("IX_InsumosResultado_CodigoCta_OportunidadeId");

        // Relacionamento com Oportunidade
        builder.HasOne(e => e.Oportunidade)
            .WithMany()
            .HasForeignKey(e => e.OportunidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
