using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.DTOs.Cliente;

namespace IntaxExterno.Application.DTOs.Proposta;

public class PropostaGetDetailsDto
{
    public int Id { get; set; }
    public string UID { get; set; } = string.Empty;
    public int? ParceiroId { get; set; }
    public ParceiroGetDto? Parceiro { get; set; }
    public int ClienteId { get; set; }
    public ClienteGetDto Cliente { get; set; } = default!;
    public List<TesesGetDto> Teses { get; set; } = new List<TesesGetDto>();
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
}
