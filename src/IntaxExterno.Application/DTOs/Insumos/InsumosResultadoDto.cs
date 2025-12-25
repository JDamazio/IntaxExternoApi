namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para o resultado do cálculo de insumos
/// </summary>
public class InsumosResultadoDto
{
    public DateTime? DataApuracao { get; set; }
    public string DescricaoVerba { get; set; } = string.Empty;
    public string CodigoCta { get; set; } = string.Empty;
    public decimal? ValorBase { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorTotal { get; set; }
}
