namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para o registro I250 do SPED Contábil - Lançamentos Contábeis
/// </summary>
public class SpedContabilI250Dto
{
    public int Id { get; set; }
    public string CodigoCta { get; set; } = string.Empty;
    public DateTime? DataApuracao { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal? Valor { get; set; }
    public int Situacao { get; set; } // 0=Active, 1=Removed, 2=Processed
    public string? IndicadorDC { get; set; } // D=Debit, C=Credit
}
