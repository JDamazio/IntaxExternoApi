using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Application.Services;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExclusaoIcmsController : ControllerBase
{
    private readonly IExclusaoIcmsService _exclusaoIcmsService;
    private readonly ISpedParserService _spedParserService;

    public ExclusaoIcmsController(
        IExclusaoIcmsService exclusaoIcmsService,
        ISpedParserService spedParserService)
    {
        _exclusaoIcmsService = exclusaoIcmsService;
        _spedParserService = spedParserService;
    }

    /// <summary>
    /// Calcula a exclusão de ICMS da base de PIS/COFINS
    /// </summary>
    /// <param name="request">Dados de contribuições e fiscal</param>
    /// <returns>Resultado do cálculo por mês</returns>
    [HttpPost("calcular")]
    public async Task<ActionResult<List<ExclusaoIcmsResultadoDto>>> CalcularExclusao(
        [FromBody] CalcularExclusaoRequestDto request)
    {
        var response = await _exclusaoIcmsService.CalcularExclusaoAsync(request);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Obtém o resultado de um cálculo já realizado
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <returns>Resultado do cálculo salvo</returns>
    [HttpGet("resultado/{oportunidadeId}")]
    public async Task<ActionResult<List<ExclusaoIcmsResultadoDto>>> GetResultado(int oportunidadeId)
    {
        var response = await _exclusaoIcmsService.GetResultadoAsync(oportunidadeId);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Upload e processamento de arquivos SPED (Contribuições e Fiscal)
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <param name="arquivoContribuicoes">Arquivo SPED Contribuições (.txt)</param>
    /// <param name="arquivoFiscal">Arquivo SPED Fiscal (.txt)</param>
    /// <returns>Resultado do cálculo</returns>
    [HttpPost("upload")]
    [RequestSizeLimit(100_000_000)] // 100MB
    public async Task<ActionResult<List<ExclusaoIcmsResultadoDto>>> UploadSpedFiles(
        [FromForm] int oportunidadeId,
        [FromForm] IFormFile? arquivoContribuicoes,
        [FromForm] IFormFile? arquivoFiscal)
    {
        try
        {
            // Validações
            if (arquivoContribuicoes == null && arquivoFiscal == null)
            {
                return BadRequest("Pelo menos um arquivo SPED deve ser enviado");
            }

            if (arquivoContribuicoes != null && !arquivoContribuicoes.FileName.EndsWith(".txt"))
            {
                return BadRequest("Arquivo SPED Contribuições deve ser .txt");
            }

            if (arquivoFiscal != null && !arquivoFiscal.FileName.EndsWith(".txt"))
            {
                return BadRequest("Arquivo SPED Fiscal deve ser .txt");
            }

            // Parse dos arquivos
            Stream? contribuicoesStream = null;
            Stream? fiscalStream = null;

            if (arquivoContribuicoes != null)
            {
                contribuicoesStream = arquivoContribuicoes.OpenReadStream();
            }

            if (arquivoFiscal != null)
            {
                fiscalStream = arquivoFiscal.OpenReadStream();
            }

            var parseResult = await _spedParserService.ParseSpedFilesAsync(
                contribuicoesStream!,
                fiscalStream!);

            // Calcular exclusão
            var request = new CalcularExclusaoRequestDto
            {
                OportunidadeId = oportunidadeId,
                DadosContribuicoes = parseResult.DadosContribuicoes,
                DadosFiscais = parseResult.DadosFiscal
            };

            var response = await _exclusaoIcmsService.CalcularExclusaoAsync(request);

            if (!response.Success)
            {
                return StatusCode(response.StatusHttp, response.Message);
            }

            return StatusCode(response.StatusHttp, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar arquivos SPED: {ex.Message}");
        }
    }
}
