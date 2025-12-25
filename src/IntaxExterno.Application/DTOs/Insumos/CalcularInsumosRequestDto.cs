namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para requisição de cálculo de insumos
/// </summary>
public class CalcularInsumosRequestDto
{
    public int OportunidadeId { get; set; }
    public List<string> ContasSelecionadas { get; set; } = new(); // Códigos das contas (I050) selecionadas
    public decimal Modalidade { get; set; } = 1.0m; // 1.0 = 100%, 0.7 = 70%
}
