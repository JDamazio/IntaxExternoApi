namespace IntaxExterno.Application.DTOs.Parceiro;

public class ParceiroPutDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Pix { get; set; } = string.Empty;
    public decimal Porcentagem { get; set; }
    public DateTime DataNascimento { get; set; }
    public string UserId { get; set; } = string.Empty;
}
