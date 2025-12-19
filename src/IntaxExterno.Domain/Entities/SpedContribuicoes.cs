namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Dados extraídos do SPED Contribuições (EFD-Contribuições)
/// </summary>
public class SpedContribuicoes : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public string CodFiscal { get; set; } = string.Empty; // CFOP
    public string CodSitPis { get; set; } = string.Empty; // CST PIS
    public decimal? AliqPis { get; set; }
    public decimal? AliqCofins { get; set; }
    public decimal? ValorIcms { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorPisCofins { get; set; }
    public DateTime? DataInicial { get; set; }
    public string Regime { get; set; } = string.Empty; // 1=Lucro Real, 2=Lucro Presumido, 3=Outros
}
