using IntaxExterno.Domain.Enums;

namespace IntaxExterno.Application.DTOs.Cliente;

public class ClientePostDto
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public RegimeTributario RegimeTributario { get; set; } = RegimeTributario.LucroPresumido;
}
