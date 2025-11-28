using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.Proposta;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PropostaController : ControllerBase
{
    private readonly IPropostaService _propostaService;
    private readonly String _token;

    public PropostaController(IPropostaService propostaService, IHttpContextAccessor httpContextAccessor)
    {
        _propostaService = propostaService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<PropostaPostDto>> Create(PropostaPostDto propostaPostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _propostaService.CreateAsync(propostaPostDto, createdById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to register proposta");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropostaGetDto>>> GetAll()
    {
        var response = await _propostaService.GetAllAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to get propostas");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PropostaGetDetailsDto>> GetById(int id)
    {
        var response = await _propostaService.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Proposta not found");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<PropostaPutDto>> Update([FromBody] PropostaPutDto propostaPutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _propostaService.UpdateAsync(propostaPutDto, updatedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to update proposta");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{propostaId}")]
    public async Task<ActionResult<bool>> Delete(int id, int propostaId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _propostaService.DeleteAsync(id, propostaId, deletedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to delete proposta");
        }
        return StatusCode(response.StatusHttp);
    }
}
