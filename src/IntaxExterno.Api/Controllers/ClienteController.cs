using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly String _token;

    public ClienteController(IClienteService clienteService, IHttpContextAccessor httpContextAccessor)
    {
        _clienteService = clienteService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<ClientePostDto>> Create(ClientePostDto clientePostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _clienteService.CreateAsync(clientePostDto, createdById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to register cliente");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteGetDto>>> GetAll()
    {
        var response = await _clienteService.GetAllAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to get clientes");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteGetDetailsDto>> GetById(int id)
    {
        var response = await _clienteService.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Cliente not found");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<ClientePutDto>> Update([FromBody] ClientePutDto clientePutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _clienteService.UpdateAsync(clientePutDto, updatedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to update cliente");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{clienteId}")]
    public async Task<ActionResult<bool>> Delete(int id, int clienteId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _clienteService.DeleteAsync(id, clienteId, deletedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to delete cliente");
        }
        return StatusCode(response.StatusHttp);
    }
}
