namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class RelatorioDeCreditoPerseGetDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public int AnoPeriodo { get; set; }
    public decimal TotalIRPJ { get; set; }
    public decimal TotalCSLL { get; set; }
    public decimal TotalPIS { get; set; }
    public decimal TotalCOFINS { get; set; }
    public decimal Total { get; set; }
    public decimal Saldo { get; set; }
    public int QuantidadeItens { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
}
