using IntaxExterno.Infra.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntaxExterno.Infra.Data.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configurações adicionais para a entidade User (além das configurações padrão do Identity)

        builder.Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Created)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(50);

        builder.Property(u => u.Updated);

        builder.Property(u => u.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(u => u.Deleted);

        builder.Property(u => u.DeletedBy)
            .HasMaxLength(50);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Índice para melhorar performance de buscas por usuários ativos
        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_User_IsActive");

        // Índice composto para auditoria
        builder.HasIndex(u => new { u.Created, u.IsActive })
            .HasDatabaseName("IX_User_Created_IsActive");
    }
}
