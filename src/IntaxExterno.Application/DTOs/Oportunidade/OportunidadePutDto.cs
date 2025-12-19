namespace IntaxExterno.Application.DTOs.Oportunidade;

public class OportunidadePutDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int? ParceiroId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFechamento { get; set; }
    public int Status { get; set; }
    public List<int> TesesIds { get; set; } = new();
}
