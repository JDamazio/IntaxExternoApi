namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class ItemRelatorioDeCreditoPerseDto
{
    public int Id { get; set; }
    public string TipoTributo { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public int? NumPedido { get; set; }
    public decimal TotalSolicitado { get; set; }
    public decimal? CorrecaoMonetaria { get; set; }
    public decimal? TotalRecebido { get; set; }
    public string? Observacao { get; set; }
}
