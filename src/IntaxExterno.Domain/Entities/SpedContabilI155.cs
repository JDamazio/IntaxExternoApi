namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Detalhamento dos saldos periódicos - Registro I155 do SPED Contábil
/// Usado para cálculo de créditos de PIS/COFINS sobre insumos
/// </summary>
public class SpedContabilI155 : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    /// <summary>
    /// Código da conta analítica (relacionado ao I050)
    /// </summary>
    public string CodCta { get; set; } = string.Empty;

    /// <summary>
    /// Código do centro de custos
    /// </summary>
    public string? CodCcus { get; set; }

    /// <summary>
    /// Valor do saldo inicial/final - Débito
    /// </summary>
    public decimal? ValorDebito { get; set; }

    /// <summary>
    /// Valor do saldo inicial/final - Crédito
    /// </summary>
    public decimal? ValorCredito { get; set; }

    /// <summary>
    /// Indicador da situação do saldo:
    /// I = Saldo Inicial
    /// F = Saldo Final
    /// D = Débito (usado para cálculo de créditos)
    /// C = Crédito
    /// </summary>
    public string IndicadorSituacao { get; set; } = string.Empty;

    /// <summary>
    /// Data de início do período
    /// </summary>
    public DateTime? DataInicio { get; set; }

    /// <summary>
    /// Data de fim do período
    /// </summary>
    public DateTime? DataFim { get; set; }
}
