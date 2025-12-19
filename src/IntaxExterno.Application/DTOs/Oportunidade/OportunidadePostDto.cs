namespace IntaxExterno.Application.DTOs.Oportunidade;

public class OportunidadePostDto
{
    public int ClienteId { get; set; }
    public int? ParceiroId { get; set; }
    public string UsuarioOrigemId { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public int Status { get; set; }
    public List<int> TesesIds { get; set; } = new();
}
