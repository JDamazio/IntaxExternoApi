namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Representa o registro I250 do SPED Contábil - Lançamentos por conta contábil
/// </summary>
public class SpedContabilI250 : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    /// <summary>
    /// Código da conta analítica
    /// </summary>
    public string CodigoCta { get; set; } = string.Empty;

    /// <summary>
    /// Data do lançamento/apuração
    /// </summary>
    public DateTime? DataApuracao { get; set; }

    /// <summary>
    /// Descrição do lançamento (histórico)
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor do lançamento
    /// </summary>
    public decimal? Valor { get; set; }

    /// <summary>
    /// Situação do registro:
    /// 0 = Ativo (será usado no cálculo)
    /// 1 = Removido (excluído pelo usuário)
    /// 2 = Processado (já calculado)
    /// </summary>
    public int Situacao { get; set; } = 0;

    /// <summary>
    /// Indicador de débito/crédito
    /// D = Débito
    /// C = Crédito
    /// </summary>
    public string? IndicadorDC { get; set; }
}
