using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TesesController : ControllerBase
{
    private readonly ITesesService _tesesService;
    private readonly String _token;

    public TesesController(ITesesService tesesService, IHttpContextAccessor httpContextAccessor)
    {
        _tesesService = tesesService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<TesesPostDto>> Create(TesesPostDto tesesPostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _tesesService.CreateAsync(tesesPostDto, createdById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to register teses");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TesesGetDto>>> GetAll()
    {
        var response = await _tesesService.GetAllAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to get teses");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TesesGetDetailsDto>> GetById(int id)
    {
        var response = await _tesesService.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Teses not found");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<TesesPutDto>> Update([FromBody] TesesPutDto tesesPutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _tesesService.UpdateAsync(tesesPutDto, updatedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to update teses");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{tesesId}")]
    public async Task<ActionResult<bool>> Delete(int id, int tesesId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _tesesService.DeleteAsync(id, tesesId, deletedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to delete teses");
        }
        return StatusCode(response.StatusHttp);
    }
}
