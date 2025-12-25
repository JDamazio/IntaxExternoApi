namespace IntaxExterno.Application.DTOs.Insumos;

public class SpedContabilI155Dto
{
    public string CodCta { get; set; } = string.Empty;
    public string? CodCcus { get; set; }
    public decimal? ValorDebito { get; set; }
    public decimal? ValorCredito { get; set; }
    public string IndicadorSituacao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}
