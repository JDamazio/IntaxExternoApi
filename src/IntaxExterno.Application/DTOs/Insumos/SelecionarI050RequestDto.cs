namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para seleção de contas do plano de contas (I050)
/// </summary>
public class SelecionarI050RequestDto
{
    public int OportunidadeId { get; set; }
    public List<int> I050Ids { get; set; } = new();
}
