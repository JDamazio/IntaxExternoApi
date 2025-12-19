using AutoMapper;
using IntaxExterno.Application.DTOs.Oportunidade;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class OportunidadeService : IOportunidadeService
{
    private readonly IOportunidadeRepository _oportunidadeRepository;
    private readonly IMapper _mapper;

    public OportunidadeService(IOportunidadeRepository oportunidadeRepository, IMapper mapper)
    {
        _oportunidadeRepository = oportunidadeRepository;
        _mapper = mapper;
    }

    public async Task<Response<OportunidadePostDto>> CreateAsync(OportunidadePostDto oportunidadePostDto, string createdById)
    {
        var oportunidade = _mapper.Map<Oportunidade>(oportunidadePostDto);
        var createdOportunidade = await _oportunidadeRepository.CreateAsync(oportunidade, createdById);

        if (createdOportunidade == null)
            return new Response<OportunidadePostDto>(false, "Failed to create oportunidade", null, 500);

        return new Response<OportunidadePostDto>(true, "Success", _mapper.Map<OportunidadePostDto>(createdOportunidade), 201);
    }

    public async Task<Response<IEnumerable<OportunidadeGetDto>>> GetAllAsync()
    {
        var oportunidades = await _oportunidadeRepository.GetAllAsync();

        if (!oportunidades.Any())
            return new Response<IEnumerable<OportunidadeGetDto>>(false, "No oportunidades found", null, 404);

        var dtos = oportunidades.Select(o => new OportunidadeGetDto
        {
            Id = o.Id,
            ClienteId = o.ClienteId,
            ClienteNome = o.Cliente?.Nome ?? string.Empty,
            ParceiroId = o.ParceiroId,
            ParceiroNome = o.Parceiro?.Nome,
            Descricao = o.Descricao,
            DataInicio = o.DataInicio,
            DataFechamento = o.DataFechamento,
            Status = (int)o.Status,
            StatusDescricao = o.Status.ToString(),
            QuantidadeTeses = o.OportunidadeTeses.Count,
            IsActive = o.IsActive,
            Created = o.Created
        });

        return new Response<IEnumerable<OportunidadeGetDto>>(true, "Success", dtos, 200);
    }

    public async Task<Response<OportunidadeGetDetailsDto?>> GetByIdAsync(int id)
    {
        var oportunidade = await _oportunidadeRepository.GetByIdAsync(id);

        if (oportunidade == null)
            return new Response<OportunidadeGetDetailsDto?>(false, "Oportunidade not found", 404);

        var dto = _mapper.Map<OportunidadeGetDetailsDto>(oportunidade);
        return new Response<OportunidadeGetDetailsDto?>(true, "Success", dto, 200);
    }

    public async Task<Response<OportunidadePutDto>> UpdateAsync(OportunidadePutDto oportunidadePutDto, string updatedById)
    {
        var oportunidade = _mapper.Map<Oportunidade>(oportunidadePutDto);
        var updatedOportunidade = await _oportunidadeRepository.UpdateAsync(oportunidade, updatedById);

        if (updatedOportunidade == null)
            return new Response<OportunidadePutDto>(false, "Oportunidade not found", 404);

        return new Response<OportunidadePutDto>(true, "Success", _mapper.Map<OportunidadePutDto>(updatedOportunidade), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int oportunidadeId, string deletedById)
    {
        var oportunidade = await _oportunidadeRepository.GetByIdAsync(id);

        if (oportunidade == null)
            return new Response<bool>(false, "Oportunidade not found", 404);

        if (oportunidade.Id != oportunidadeId)
            return new Response<bool>(false, "You are not allowed to delete this oportunidade", 403);

        bool result = await _oportunidadeRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }
}
