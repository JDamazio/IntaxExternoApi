using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.Oportunidade;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OportunidadeController : ControllerBase
{
    private readonly IOportunidadeService _oportunidadeService;
    private readonly string _token;

    public OportunidadeController(IOportunidadeService oportunidadeService, IHttpContextAccessor httpContextAccessor)
    {
        _oportunidadeService = oportunidadeService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<OportunidadePostDto>> Create(OportunidadePostDto oportunidadePostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _oportunidadeService.CreateAsync(oportunidadePostDto, createdById);

        if (!response.Success)
            return StatusCode(response.StatusHttp, "Failed to register oportunidade");

        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OportunidadeGetDto>>> GetAll()
    {
        var response = await _oportunidadeService.GetAllAsync();

        if (!response.Success)
            return StatusCode(response.StatusHttp, "Failed to get oportunidades");

        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OportunidadeGetDetailsDto>> GetById(int id)
    {
        var response = await _oportunidadeService.GetByIdAsync(id);

        if (!response.Success)
            return StatusCode(response.StatusHttp, "Oportunidade not found");

        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<OportunidadePutDto>> Update([FromBody] OportunidadePutDto oportunidadePutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _oportunidadeService.UpdateAsync(oportunidadePutDto, updatedById);

        if (!response.Success)
            return StatusCode(response.StatusHttp, "Failed to update oportunidade");

        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{oportunidadeId}")]
    public async Task<ActionResult<bool>> Delete(int id, int oportunidadeId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _oportunidadeService.DeleteAsync(id, oportunidadeId, deletedById);

        if (!response.Success)
            return StatusCode(response.StatusHttp, "Failed to delete oportunidade");

        return StatusCode(response.StatusHttp);
    }
}
