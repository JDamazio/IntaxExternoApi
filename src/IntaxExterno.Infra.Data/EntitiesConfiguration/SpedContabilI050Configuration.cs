using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class SpedContabilI050Configuration : IEntityTypeConfiguration<SpedContabilI050>
{
    public void Configure(EntityTypeBuilder<SpedContabilI050> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UID)
            .HasMaxLength(36)
            .IsRequired();

        builder.HasIndex(e => e.UID)
            .IsUnique()
            .HasDatabaseName("IX_SpedContabilI050_UID");

        builder.Property(e => e.OportunidadeId)
            .IsRequired();

        builder.Property(e => e.DataInicial)
            .IsRequired(false);

        builder.Property(e => e.DataFinal)
            .IsRequired(false);

        builder.Property(e => e.CodigoCta)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.NomeCta)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.CodNatureza)
            .HasMaxLength(2)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.QtdI250)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.QtdI250Selecionados)
            .IsRequired()
            .HasDefaultValue(0);

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
            .HasDatabaseName("IX_SpedContabilI050_IsActive");

        builder.HasIndex(e => new { e.OportunidadeId, e.IsActive })
            .HasDatabaseName("IX_SpedContabilI050_OportunidadeId_IsActive");

        builder.HasIndex(e => new { e.OportunidadeId, e.CodigoCta })
            .HasDatabaseName("IX_SpedContabilI050_OportunidadeId_CodigoCta");

        builder.HasIndex(e => new { e.OportunidadeId, e.CodNatureza })
            .HasDatabaseName("IX_SpedContabilI050_OportunidadeId_CodNatureza");

        builder.HasIndex(e => new { e.OportunidadeId, e.Status })
            .HasDatabaseName("IX_SpedContabilI050_OportunidadeId_Status");

        // Relacionamento com Oportunidade
        builder.HasOne(e => e.Oportunidade)
            .WithMany()
            .HasForeignKey(e => e.OportunidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
