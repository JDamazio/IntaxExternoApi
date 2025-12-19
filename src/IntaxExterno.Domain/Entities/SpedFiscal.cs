namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Dados extraídos do SPED Fiscal (EFD-ICMS/IPI)
/// </summary>
public class SpedFiscal : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public string Cfop { get; set; } = string.Empty;
    public string CstIcms { get; set; } = string.Empty;
    public decimal? AliqPis { get; set; }
    public decimal? AliqCofins { get; set; }
    public decimal? ValorIcms { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorPisCofins { get; set; }
    public DateTime? DataInicial { get; set; }
}
