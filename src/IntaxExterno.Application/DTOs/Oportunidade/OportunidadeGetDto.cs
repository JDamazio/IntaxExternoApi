namespace IntaxExterno.Application.DTOs.Oportunidade;

public class OportunidadeGetDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public int? ParceiroId { get; set; }
    public string? ParceiroNome { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFechamento { get; set; }
    public int Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public int QuantidadeTeses { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
}
