using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        // Configuração da chave primária (herdada de BaseEntity)
        builder.HasKey(c => c.Id);

        // Configuração da propriedade UID (herdada de BaseEntity)
        builder.Property(c => c.UID)
            .HasMaxLength(36)
            .IsRequired();

        // Configuração das propriedades específicas de Cliente
        builder.Property(c => c.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Telefone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.EmailResponsavel)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.CNPJ)
            .HasMaxLength(18)
            .IsRequired();

        // Configurações de auditoria (herdadas de BaseEntity)
        builder.Property(c => c.Created)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(50);

        builder.Property(c => c.Updated);

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(c => c.Deleted);

        builder.Property(c => c.DeletedBy)
            .HasMaxLength(50);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Índices para melhorar performance
        builder.HasIndex(c => c.CNPJ)
            .IsUnique()
            .HasDatabaseName("IX_Cliente_CNPJ");

        builder.HasIndex(c => c.Email)
            .HasDatabaseName("IX_Cliente_Email");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Cliente_IsActive");

        builder.HasIndex(c => c.UID)
            .IsUnique()
            .HasDatabaseName("IX_Cliente_UID");
    }
}
