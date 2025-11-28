using IntaxExterno.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class PropostaTesesConfiguration : IEntityTypeConfiguration<PropostaTeses>
{
    public void Configure(EntityTypeBuilder<PropostaTeses> builder)
    {
        // Configuração da chave primária (herdada de BaseEntity)
        builder.HasKey(pt => pt.Id);

        // Configuração da propriedade UID (herdada de BaseEntity)
        builder.Property(pt => pt.UID)
            .HasMaxLength(36)
            .IsRequired();

        // Configurações de auditoria (herdadas de BaseEntity)
        builder.Property(pt => pt.Created)
            .IsRequired();

        builder.Property(pt => pt.CreatedBy)
            .HasMaxLength(50);

        builder.Property(pt => pt.Updated);

        builder.Property(pt => pt.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(pt => pt.Deleted);

        builder.Property(pt => pt.DeletedBy)
            .HasMaxLength(50);

        builder.Property(pt => pt.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Relacionamento com Proposta
        builder.HasOne(pt => pt.Proposta)
            .WithMany(p => p.PropostaTeses)
            .HasForeignKey(pt => pt.PropostaId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Relacionamento com Teses
        builder.HasOne(pt => pt.Teses)
            .WithMany()
            .HasForeignKey(pt => pt.TesesId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Índices para melhorar performance
        builder.HasIndex(pt => pt.PropostaId)
            .HasDatabaseName("IX_PropostaTeses_PropostaId");

        builder.HasIndex(pt => pt.TesesId)
            .HasDatabaseName("IX_PropostaTeses_TesesId");

        builder.HasIndex(pt => pt.IsActive)
            .HasDatabaseName("IX_PropostaTeses_IsActive");

        builder.HasIndex(pt => pt.UID)
            .IsUnique()
            .HasDatabaseName("IX_PropostaTeses_UID");

        // Índice composto para evitar duplicatas
        builder.HasIndex(pt => new { pt.PropostaId, pt.TesesId })
            .IsUnique()
            .HasDatabaseName("IX_PropostaTeses_PropostaId_TesesId");
    }
}
