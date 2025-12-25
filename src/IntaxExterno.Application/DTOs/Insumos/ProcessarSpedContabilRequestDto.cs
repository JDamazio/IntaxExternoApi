using Microsoft.AspNetCore.Http;

namespace IntaxExterno.Application.DTOs.Insumos;

/// <summary>
/// DTO para requisição de processamento de arquivo SPED Contábil
/// </summary>
public class ProcessarSpedContabilRequestDto
{
    public int OportunidadeId { get; set; }
    public IFormFile Arquivo { get; set; } = default!;
}
