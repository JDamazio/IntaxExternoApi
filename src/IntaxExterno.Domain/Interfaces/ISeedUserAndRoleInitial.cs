namespace IntaxExterno.Domain.Interfaces;

/// <summary>
/// Interface para seed de usuários e roles iniciais
/// </summary>
public interface ISeedUserAndRoleInitial
{
    /// <summary>
    /// Faz seed de todas as roles do sistema
    /// </summary>
    void SeedRoles();

    /// <summary>
    /// Faz seed de usuários de teste para cada role
    /// </summary>
    void SeedUsers();
}
