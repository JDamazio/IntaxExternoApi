namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Resultado do cálculo de exclusão de ICMS da base de PIS/COFINS
/// </summary>
public class ExclusaoIcmsResultado : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public DateTime? DataInicial { get; set; }
    public decimal? ValorIcms { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorPisCofins { get; set; }
}
