using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ParceiroController : ControllerBase
{
    private readonly IParceiroService _parceiroService;
    private readonly String _token;

    public ParceiroController(IParceiroService parceiroService, IHttpContextAccessor httpContextAccessor)
    {
        _parceiroService = parceiroService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<ParceiroPostDto>> Create(ParceiroPostDto parceiroPostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _parceiroService.CreateAsync(parceiroPostDto, createdById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to register parceiro");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParceiroGetDto>>> GetAll()
    {
        var response = await _parceiroService.GetAllAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to get parceiros");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParceiroGetDetailsDto>> GetById(int id)
    {
        var response = await _parceiroService.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Parceiro not found");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<ParceiroPutDto>> Update([FromBody] ParceiroPutDto parceiroPutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _parceiroService.UpdateAsync(parceiroPutDto, updatedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to update parceiro");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{parceiroId}")]
    public async Task<ActionResult<bool>> Delete(int id, int parceiroId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _parceiroService.DeleteAsync(id, parceiroId, deletedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to delete parceiro");
        }
        return StatusCode(response.StatusHttp);
    }
}
