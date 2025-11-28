namespace IntaxExterno.Application.DTOs.Proposta;

public class PropostaPutDto
{
    public int Id { get; set; }
    public int? ParceiroId { get; set; }
    public int ClienteId { get; set; }
    public List<int> TesesIds { get; set; } = new List<int>();
}
