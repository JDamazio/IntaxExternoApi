namespace IntaxExterno.Application.DTOs.Parceiro;

public class ParceiroGetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Porcentagem { get; set; }
    public bool IsActive { get; set; }
}
