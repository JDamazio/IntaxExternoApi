namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Relacionamento N:N entre Oportunidade e Teses
/// </summary>
public class OportunidadeTeses : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public int TesesId { get; set; }
    public Teses Teses { get; set; } = default!;

    // Informações adicionais sobre a tese aplicada nesta oportunidade
    public string? Observacoes { get; set; }
    public decimal? ValorEstimado { get; set; }
}
