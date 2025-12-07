namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class RelatorioDeCreditoPersePostDto
{
    public int ClienteId { get; set; }
    public DateTime DataEmissao { get; set; }
    public decimal TotalIRPJ { get; set; }
    public decimal TotalCSLL { get; set; }
    public decimal TotalPIS { get; set; }
    public decimal TotalCOFINS { get; set; }
    public decimal Total { get; set; }
    public decimal Saldo { get; set; }
    public List<ItemRelatorioDeCreditoPerseDto> Itens { get; set; } = new List<ItemRelatorioDeCreditoPerseDto>();
}
