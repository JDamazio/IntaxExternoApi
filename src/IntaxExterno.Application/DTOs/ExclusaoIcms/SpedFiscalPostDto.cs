namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class SpedFiscalPostDto
{
    public int OportunidadeId { get; set; }
    public string Cfop { get; set; } = string.Empty;
    public string CstIcms { get; set; } = string.Empty;
    public decimal? ValorIcms { get; set; }
    public DateTime? DataInicial { get; set; }
}
