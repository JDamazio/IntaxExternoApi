namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para o registro I050 do SPED Contábil - Plano de Contas
/// </summary>
public class SpedContabilI050Dto
{
    public int Id { get; set; }
    public string CodigoCta { get; set; } = string.Empty;
    public string NomeCta { get; set; } = string.Empty;
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public string? CodNatureza { get; set; }
    public int Status { get; set; }
    public int QtdI250 { get; set; }
    public int QtdI250Selecionados { get; set; }
    public decimal? ValorTotal { get; set; } // Soma dos I250s ativos
}
