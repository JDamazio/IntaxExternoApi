namespace IntaxExterno.Application.DTOs.Proposta;

public class PropostaGetDto
{
    public int Id { get; set; }
    public int? ParceiroId { get; set; }
    public string? ParceiroNome { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public int QuantidadeTeses { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
}
