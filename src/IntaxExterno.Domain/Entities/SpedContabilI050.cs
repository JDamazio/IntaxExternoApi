namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Representa o registro I050 do SPED Contábil - Plano de Contas
/// </summary>
public class SpedContabilI050 : BaseEntity
{
    public int OportunidadeId { get; set; }
    public Oportunidade Oportunidade { get; set; } = default!;

    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }

    /// <summary>
    /// Código da conta analítica
    /// </summary>
    public string CodigoCta { get; set; } = string.Empty;

    /// <summary>
    /// Nome da conta contábil
    /// </summary>
    public string NomeCta { get; set; } = string.Empty;

    /// <summary>
    /// Código de natureza da conta:
    /// 01 = Contas de ativo
    /// 02 = Contas de passivo
    /// 03 = Patrimônio líquido
    /// 04 = Contas de resultado (DESPESAS - usado para insumos)
    /// 05 = Contas de compensação
    /// 09 = Outras
    /// </summary>
    public string? CodNatureza { get; set; }

    /// <summary>
    /// Situação/Status do processamento:
    /// 0 = Pendente
    /// 1 = Não visualizado
    /// 2 = Selecionado
    /// 3 = Finalizado
    /// 4 = Erro
    /// 6 = Processando
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// Quantidade de I250s vinculados a esta conta
    /// </summary>
    public int QtdI250 { get; set; } = 0;

    /// <summary>
    /// Quantidade de I250s ativos (Situacao = 0)
    /// </summary>
    public int QtdI250Selecionados { get; set; } = 0;
}
