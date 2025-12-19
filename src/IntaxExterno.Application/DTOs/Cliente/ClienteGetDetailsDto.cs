using IntaxExterno.Domain.Enums;

namespace IntaxExterno.Application.DTOs.Cliente;

public class ClienteGetDetailsDto
{
    public int Id { get; set; }
    public string UID { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public RegimeTributario RegimeTributario { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
}
