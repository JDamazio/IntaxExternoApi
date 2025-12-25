namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para seleção de insumos (I250)
/// </summary>
public class SelecionarI250RequestDto
{
    public int OportunidadeId { get; set; }
    public string CodigoCta { get; set; } = string.Empty;
    public List<int> I250Ids { get; set; } = new();
}
