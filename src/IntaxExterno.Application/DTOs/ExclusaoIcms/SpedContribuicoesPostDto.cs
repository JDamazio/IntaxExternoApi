namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class SpedContribuicoesPostDto
{
    public int OportunidadeId { get; set; }
    public string CodFiscal { get; set; } = string.Empty; // CFOP
    public string CodSitPis { get; set; } = string.Empty; // CST PIS
    public decimal? AliqPis { get; set; }
    public decimal? AliqCofins { get; set; }
    public decimal? ValorIcms { get; set; }
    public DateTime? DataInicial { get; set; }
    public string Regime { get; set; } = string.Empty;
}
