namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class SpedContribuicoesGetDto
{
    public int Id { get; set; }
    public int OportunidadeId { get; set; }
    public string CodFiscal { get; set; } = string.Empty;
    public string CodSitPis { get; set; } = string.Empty;
    public decimal? AliqPis { get; set; }
    public decimal? AliqCofins { get; set; }
    public decimal? ValorIcms { get; set; }
    public DateTime? DataInicial { get; set; }
    public string Regime { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
