namespace IntaxExterno.Domain.Entities;

/// <summary>
/// Representa uma oportunidade de análise tributária
/// </summary>
public class Oportunidade : BaseEntity
{
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = default!;

    public int? ParceiroId { get; set; }
    public Parceiro? Parceiro { get; set; }

    // Usuário que trouxe o cliente
    public string UsuarioOrigemId { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public DateTime? DataInicio { get; set; }
    public DateTime? DataFechamento { get; set; }

    // Status da oportunidade
    public StatusOportunidade Status { get; set; }

    // Relacionamentos
    public ICollection<OportunidadeTeses> OportunidadeTeses { get; set; } = new List<OportunidadeTeses>();
    public ICollection<ExclusaoIcmsResultado> ResultadosExclusaoIcms { get; set; } = new List<ExclusaoIcmsResultado>();
    public ICollection<SpedContribuicoes> SpedContribuicoes { get; set; } = new List<SpedContribuicoes>();
    public ICollection<SpedFiscal> SpedFiscais { get; set; } = new List<SpedFiscal>();
}
