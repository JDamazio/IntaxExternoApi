using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.DTOs.Teses;

namespace IntaxExterno.Application.DTOs.Oportunidade;

public class OportunidadeGetDetailsDto
{
    public int Id { get; set; }
    public string UID { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public ClienteGetDto? Cliente { get; set; }
    public int? ParceiroId { get; set; }
    public ParceiroGetDto? Parceiro { get; set; }
    public string UsuarioOrigemId { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFechamento { get; set; }
    public int Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public List<TesesGetDto> Teses { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
}
