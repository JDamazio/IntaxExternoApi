using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Application.DTOs.Insumos;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InsumosController : ControllerBase
{
    private readonly IInsumosService _insumosService;

    public InsumosController(IInsumosService insumosService)
    {
        _insumosService = insumosService;
    }

    /// <summary>
    /// Upload e processamento de arquivo SPED Contábil
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <param name="arquivo">Arquivo SPED Contábil (.txt)</param>
    /// <returns>Resultado do processamento</returns>
    [HttpPost("upload")]
    [RequestSizeLimit(100_000_000)] // 100MB
    public async Task<ActionResult<bool>> UploadSpedContabil(
        [FromForm] int oportunidadeId,
        [FromForm] IFormFile arquivo)
    {
        try
        {
            // Validações
            if (arquivo == null)
            {
                return BadRequest("Arquivo SPED Contábil deve ser enviado");
            }

            if (!arquivo.FileName.EndsWith(".txt"))
            {
                return BadRequest("Arquivo SPED Contábil deve ser .txt");
            }

            // Processar arquivo
            using var stream = arquivo.OpenReadStream();
            var response = await _insumosService.ProcessarSpedContabilAsync(oportunidadeId, stream);

            if (!response.Success)
            {
                return StatusCode(response.StatusHttp, response.Message);
            }

            return StatusCode(response.StatusHttp, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar arquivo SPED Contábil: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém o plano de contas (I050) de uma oportunidade
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <returns>Lista do plano de contas</returns>
    [HttpGet("plano-contas/{oportunidadeId}")]
    public async Task<ActionResult<List<SpedContabilI050Dto>>> GetPlanoContas(int oportunidadeId)
    {
        var response = await _insumosService.GetPlanoContasAsync(oportunidadeId);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Obtém os insumos (I250) de uma conta contábil
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <param name="codigoCta">Código da conta contábil</param>
    /// <returns>Lista de insumos da conta</returns>
    [HttpGet("insumos/{oportunidadeId}/{codigoCta}")]
    public async Task<ActionResult<List<SpedContabilI250Dto>>> GetInsumosByConta(
        int oportunidadeId,
        string codigoCta)
    {
        var response = await _insumosService.GetInsumosByContaAsync(oportunidadeId, codigoCta);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Seleciona contas do plano de contas (Passo 2)
    /// </summary>
    /// <param name="request">IDs das contas a selecionar</param>
    /// <returns>Confirmação da seleção</returns>
    [HttpPost("selecionar-i050")]
    public async Task<ActionResult<bool>> SelecionarI050([FromBody] SelecionarI050RequestDto request)
    {
        var response = await _insumosService.SelecionarI050Async(request);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Seleciona insumos de uma conta (Passo 3)
    /// </summary>
    /// <param name="request">IDs dos insumos a selecionar</param>
    /// <returns>Confirmação da seleção</returns>
    [HttpPost("selecionar-i250")]
    public async Task<ActionResult<bool>> SelecionarI250([FromBody] SelecionarI250RequestDto request)
    {
        var response = await _insumosService.SelecionarI250Async(request);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Calcula os créditos de PIS/COFINS sobre insumos
    /// </summary>
    /// <param name="request">ID da oportunidade e insumos selecionados</param>
    /// <returns>Resultado do cálculo</returns>
    [HttpPost("calcular")]
    public async Task<ActionResult<List<InsumosResultadoDto>>> CalcularInsumos(
        [FromBody] CalcularInsumosRequestDto request)
    {
        var response = await _insumosService.CalcularInsumosAsync(request);

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
    public async Task<ActionResult<List<InsumosResultadoDto>>> GetResultado(int oportunidadeId)
    {
        var response = await _insumosService.GetResultadoAsync(oportunidadeId);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Exporta o resultado de insumos para Excel
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <returns>Arquivo Excel com os resultados</returns>
    [HttpGet("export/excel/{oportunidadeId}")]
    public async Task<ActionResult> ExportToExcel(int oportunidadeId)
    {
        var response = await _insumosService.ExportToExcelAsync(oportunidadeId);

        if (!response.Success || response.Object == null)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        var oportunidade = await _insumosService.GetResultadoAsync(oportunidadeId);
        var fileName = $"Insumos_Oportunidade_{oportunidadeId}_{DateTime.Now:yyyyMMdd}.xlsx";

        return File(response.Object, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    /// <summary>
    /// Exporta o resultado de insumos para PDF
    /// </summary>
    /// <param name="oportunidadeId">ID da oportunidade</param>
    /// <returns>Arquivo PDF com os resultados</returns>
    [HttpGet("export/pdf/{oportunidadeId}")]
    public async Task<ActionResult> ExportToPdf(int oportunidadeId)
    {
        var response = await _insumosService.ExportToPdfAsync(oportunidadeId);

        if (!response.Success || response.Object == null)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        var fileName = $"Insumos_Oportunidade_{oportunidadeId}_{DateTime.Now:yyyyMMdd}.pdf";

        return File(response.Object, "application/pdf", fileName);
    }
}
