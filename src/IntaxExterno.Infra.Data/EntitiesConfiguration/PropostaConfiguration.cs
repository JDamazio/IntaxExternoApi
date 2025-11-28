using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class PropostaConfiguration : IEntityTypeConfiguration<Proposta>
{
    public void Configure(EntityTypeBuilder<Proposta> builder)
    {
        // Configuração da chave primária (herdada de BaseEntity)
        builder.HasKey(p => p.Id);

        // Configuração da propriedade UID (herdada de BaseEntity)
        builder.Property(p => p.UID)
            .HasMaxLength(36)
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

        // Relacionamento com Cliente (obrigatório)
        builder.HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Relacionamento com Parceiro (opcional)
        builder.HasOne(p => p.Parceiro)
            .WithMany()
            .HasForeignKey(p => p.ParceiroId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Relacionamento com PropostaTeses (many-to-many através de tabela intermediária)
        builder.HasMany(p => p.PropostaTeses)
            .WithOne(pt => pt.Proposta)
            .HasForeignKey(pt => pt.PropostaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices para melhorar performance
        builder.HasIndex(p => p.ClienteId)
            .HasDatabaseName("IX_Proposta_ClienteId");

        builder.HasIndex(p => p.ParceiroId)
            .HasDatabaseName("IX_Proposta_ParceiroId");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Proposta_IsActive");

        builder.HasIndex(p => p.UID)
            .IsUnique()
            .HasDatabaseName("IX_Proposta_UID");

        // Índice composto para queries comuns
        builder.HasIndex(p => new { p.ClienteId, p.IsActive })
            .HasDatabaseName("IX_Proposta_ClienteId_IsActive");
    }
}
