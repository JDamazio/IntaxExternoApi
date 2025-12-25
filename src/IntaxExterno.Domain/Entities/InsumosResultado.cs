namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Resultado do cálculo de créditos de PIS/COFINS sobre insumos
/// </summary>
public class InsumosResultado : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public DateTime? DataApuracao { get; set; }
    public string DescricaoVerba { get; set; } = string.Empty;
    public string CodigoCta { get; set; } = string.Empty;

    public decimal? ValorBase { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorTotal { get; set; }
}
