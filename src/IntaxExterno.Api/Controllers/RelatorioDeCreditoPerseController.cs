using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntaxExterno.Api.Helpers.Jwt;
using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using IntaxExterno.Application.Interfaces;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RelatorioDeCreditoPerseController : ControllerBase
{
    private readonly IRelatorioDeCreditoPerseService _relatorioDeCreditoPerseService;
    private readonly String _token;

    public RelatorioDeCreditoPerseController(IRelatorioDeCreditoPerseService relatorioDeCreditoPerseService, IHttpContextAccessor httpContextAccessor)
    {
        _relatorioDeCreditoPerseService = relatorioDeCreditoPerseService;
        _token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "") ?? string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<RelatorioDeCreditoPersePostDto>> Create(RelatorioDeCreditoPersePostDto relatorioDeCreditoPersePostDto)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);
        var response = await _relatorioDeCreditoPerseService.CreateAsync(relatorioDeCreditoPersePostDto, createdById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to register relatorioDeCreditoPerse");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelatorioDeCreditoPerseGetDto>>> GetAll()
    {
        var response = await _relatorioDeCreditoPerseService.GetAllAsync();
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to get relatorioDeCreditoPerses");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RelatorioDeCreditoPerseGetDetailsDto>> GetById(int id)
    {
        var response = await _relatorioDeCreditoPerseService.GetByIdAsync(id);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "RelatorioDeCreditoPerse not found");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpPut]
    public async Task<ActionResult<RelatorioDeCreditoPersePutDto>> Update([FromBody] RelatorioDeCreditoPersePutDto relatorioDeCreditoPersePutDto)
    {
        string updatedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _relatorioDeCreditoPerseService.UpdateAsync(relatorioDeCreditoPersePutDto, updatedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to update relatorioDeCreditoPerse");
        }
        return StatusCode(response.StatusHttp, response);
    }

    [HttpDelete("{id}/{relatorioDeCreditoPerseId}")]
    public async Task<ActionResult<bool>> Delete(int id, int relatorioDeCreditoPerseId)
    {
        string deletedById = JwtHelper.GetUserIdFromToken(_token);

        var response = await _relatorioDeCreditoPerseService.DeleteAsync(id, relatorioDeCreditoPerseId, deletedById);
        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, "Failed to delete relatorioDeCreditoPerse");
        }
        return StatusCode(response.StatusHttp);
    }

    [HttpPost("import")]
    public async Task<ActionResult> ImportFromExcel(IFormFile file, [FromQuery] DateTime dataEmissao)
    {
        string createdById = JwtHelper.GetUserIdFromToken(_token);

        if (file == null || file.Length == 0)
        {
            return BadRequest("Arquivo não fornecido");
        }

        var response = await _relatorioDeCreditoPerseService.ImportFromExcelAsync(file, createdById, dataEmissao);

        if (!response.Success)
        {
            return StatusCode(response.StatusHttp, response);
        }

        return StatusCode(response.StatusHttp, response);
    }

    [HttpGet("export/{id}")]
    public async Task<ActionResult> ExportToExcel(int id)
    {
        var response = await _relatorioDeCreditoPerseService.ExportToExcelAsync(id);

        if (!response.Success || response.Object == null)
        {
            return StatusCode(response.StatusHttp, response.Message);
        }

        var relatorio = await _relatorioDeCreditoPerseService.GetByIdAsync(id);
        var fileName = $"Relatorio_Credito_{relatorio.Object?.Cliente?.Nome ?? "Cliente"}_{DateTime.Now:yyyyMMdd}.xlsx";

        return File(response.Object, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
