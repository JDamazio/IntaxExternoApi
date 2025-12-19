using IntaxExterno.Domain.Enums;

namespace IntaxExterno.Application.DTOs.Cliente;

public class ClienteGetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RegimeTributario RegimeTributario { get; set; }
    public bool IsActive { get; set; }
}
