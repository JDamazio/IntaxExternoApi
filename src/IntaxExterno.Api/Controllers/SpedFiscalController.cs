using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SpedFiscalController : ControllerBase
{
    private readonly ISpedFiscalService _service;
    private readonly string _token;

    public SpedFiscalController(ISpedFiscalService service, IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    /// <summary>
    /// Cria múltiplos registros de SPED Fiscal de uma vez
    /// </summary>
    [HttpPost("batch")]
    public async Task<ActionResult> CreateMany([FromBody] List<SpedFiscalPostDto> dtos)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _service.CreateManyAsync(dtos, createdById);

        if (!response.Success)
            return StatusCode(response.StatusHttp, response.Message);

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Obtém todos os registros de SPED Fiscal de uma Oportunidade
    /// </summary>
    [HttpGet("oportunidade/{oportunidadeId}")]
    public async Task<ActionResult> GetByOportunidadeId(int oportunidadeId)
    {
        var response = await _service.GetByOportunidadeIdAsync(oportunidadeId);

        if (!response.Success)
            return StatusCode(response.StatusHttp, response.Message);

        return StatusCode(response.StatusHttp, response);
    }

    /// <summary>
    /// Deleta todos os registros de SPED Fiscal de uma Oportunidade
    /// </summary>
    [HttpDelete("oportunidade/{oportunidadeId}")]
    public async Task<ActionResult> DeleteByOportunidadeId(int oportunidadeId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _service.DeleteByOportunidadeIdAsync(oportunidadeId, deletedById);

        if (!response.Success)
            return StatusCode(response.StatusHttp, response.Message);

        return StatusCode(response.StatusHttp, response);
    }
}
