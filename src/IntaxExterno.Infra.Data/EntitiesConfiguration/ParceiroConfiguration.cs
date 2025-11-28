using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class ParceiroConfiguration : IEntityTypeConfiguration<Parceiro>
{
    public void Configure(EntityTypeBuilder<Parceiro> builder)
    {
        // Configuração da chave primária (herdada de BaseEntity)
        builder.HasKey(p => p.Id);

        // Configuração da propriedade UID (herdada de BaseEntity)
        builder.Property(p => p.UID)
            .HasMaxLength(36)
            .IsRequired();

        // Configuração das propriedades específicas de Parceiro
        builder.Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.CPF)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(p => p.Telefone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Pix)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Porcentagem)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(p => p.DataNascimento)
            .IsRequired();

        // Configurações de auditoria (herdadas de BaseEntity)
        builder.Property(p => p.Created)
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(50);

        builder.Property(p => p.Updated);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(p => p.Deleted);

        builder.Property(p => p.DeletedBy)
            .HasMaxLength(50);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Relacionamento com User (sem propriedade de navegação na entidade de domínio)
        builder.HasOne<Identity.User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Índices para melhorar performance
        builder.HasIndex(p => p.CPF)
            .IsUnique()
            .HasDatabaseName("IX_Parceiro_CPF");

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("IX_Parceiro_Email");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Parceiro_IsActive");

        builder.HasIndex(p => p.UID)
            .IsUnique()
            .HasDatabaseName("IX_Parceiro_UID");
    }
}
