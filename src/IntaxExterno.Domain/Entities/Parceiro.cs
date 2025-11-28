namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Entidade que representa um Parceiro do sistema
/// </summary>
public class Parceiro : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Pix { get; set; } = string.Empty;
    public decimal Porcentagem { get; set; }
    public DateTime DataNascimento { get; set; }

    // Relacionamento com User (um parceiro tem um usuário)
    public string UserId { get; set; } = string.Empty;
}
