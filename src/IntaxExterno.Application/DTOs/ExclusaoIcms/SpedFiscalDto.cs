namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class SpedFiscalDto
{
    public string Cfop { get; set; } = string.Empty;
    public string CstIcms { get; set; } = string.Empty;
    public decimal? ValorIcms { get; set; }
    public DateTime? DataInicial { get; set; }
}
