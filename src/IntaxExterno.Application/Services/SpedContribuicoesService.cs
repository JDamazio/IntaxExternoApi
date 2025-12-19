using AutoMapper;
using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class SpedContribuicoesService : ISpedContribuicoesService
{
    private readonly ISpedContribuicoesRepository _repository;
    private readonly IMapper _mapper;

    public SpedContribuicoesService(ISpedContribuicoesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<List<SpedContribuicoesGetDto>>> CreateManyAsync(List<SpedContribuicoesPostDto> dtos, string createdById)
    {
        var entities = dtos.Select(dto => _mapper.Map<SpedContribuicoes>(dto)).ToList();
        var created = await _repository.CreateManyAsync(entities, createdById);

        var result = created.Select(e => _mapper.Map<SpedContribuicoesGetDto>(e)).ToList();

        return new Response<List<SpedContribuicoesGetDto>>(
            true,
            $"{result.Count} registros de SPED Contribuições criados com sucesso",
            result,
            201
        );
    }

    public async Task<Response<List<SpedContribuicoesGetDto>>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        var entities = await _repository.GetByOportunidadeIdAsync(oportunidadeId);
        var dtos = entities.Select(e => _mapper.Map<SpedContribuicoesGetDto>(e)).ToList();

        if (!dtos.Any())
            return new Response<List<SpedContribuicoesGetDto>>(
                false,
                "Nenhum registro de SPED Contribuições encontrado para esta Oportunidade",
                404
            );

        return new Response<List<SpedContribuicoesGetDto>>(
            true,
            "Sucesso",
            dtos,
            200
        );
    }

    public async Task<Response<bool>> DeleteByOportunidadeIdAsync(int oportunidadeId, string deletedById)
    {
        var result = await _repository.DeleteByOportunidadeIdAsync(oportunidadeId, deletedById);

        if (!result)
            return new Response<bool>(
                false,
                "Nenhum registro encontrado para deletar",
                404
            );

        return new Response<bool>(
            true,
            "Registros deletados com sucesso",
            200
        );
    }
}
