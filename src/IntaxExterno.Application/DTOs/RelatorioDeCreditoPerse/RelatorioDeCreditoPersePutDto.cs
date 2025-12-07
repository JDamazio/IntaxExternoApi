namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class RelatorioDeCreditoPersePutDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int AnoPeriodo { get; set; }
    public decimal TotalIRPJ { get; set; }
    public decimal TotalCSLL { get; set; }
    public decimal TotalPIS { get; set; }
    public decimal TotalCOFINS { get; set; }
    public decimal Total { get; set; }
    public decimal Saldo { get; set; }
    public List<ItemRelatorioDeCreditoPerseDto> Itens { get; set; } = new List<ItemRelatorioDeCreditoPerseDto>();
}
