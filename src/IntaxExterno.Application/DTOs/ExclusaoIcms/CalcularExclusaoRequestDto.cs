namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class  CalcularExclusaoRequestDto
{
    public int OportunidadeId { get; set; }
    public List<SpedContribuicoesDto> DadosContribuicoes { get; set; } = new();
    public List<SpedFiscalDto> DadosFiscais { get; set; } = new();
}
