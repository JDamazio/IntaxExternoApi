using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class TesesConfiguration : IEntityTypeConfiguration<Teses>
{
    public void Configure(EntityTypeBuilder<Teses> builder)
    {
        // Configuração da chave primária (herdada de BaseEntity)
        builder.HasKey(t => t.Id);

        // Configuração da propriedade UID (herdada de BaseEntity)
        builder.Property(t => t.UID)
            .HasMaxLength(36)
            .IsRequired();

        // Configuração das propriedades específicas de Teses
        builder.Property(t => t.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Descrição)
            .HasMaxLength(500)
            .IsRequired();

        // Configurações de auditoria (herdadas de BaseEntity)
        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(50);

        builder.Property(t => t.Updated);

        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(t => t.Deleted);

        builder.Property(t => t.DeletedBy)
            .HasMaxLength(50);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Índices para melhorar performance
        builder.HasIndex(t => t.Nome)
            .HasDatabaseName("IX_Teses_Nome");

        builder.HasIndex(t => t.IsActive)
            .HasDatabaseName("IX_Teses_IsActive");

        builder.HasIndex(t => t.UID)
            .IsUnique()
            .HasDatabaseName("IX_Teses_UID");
    }
}
