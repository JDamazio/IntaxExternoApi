using IntaxExterno.Application.DTOs.Cliente;

namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class RelatorioDeCreditoPerseGetDetailsDto
{
    public int Id { get; set; }
    public string UID { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public ClienteGetDto Cliente { get; set; } = default!;
    public DateTime DataEmissao { get; set; }
    public decimal TotalIRPJ { get; set; }
    public decimal TotalCSLL { get; set; }
    public decimal TotalPIS { get; set; }
    public decimal TotalCOFINS { get; set; }
    public decimal Total { get; set; }
    public decimal Saldo { get; set; }
    public List<ItemRelatorioDeCreditoPerseDto> Itens { get; set; } = new List<ItemRelatorioDeCreditoPerseDto>();
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
}
