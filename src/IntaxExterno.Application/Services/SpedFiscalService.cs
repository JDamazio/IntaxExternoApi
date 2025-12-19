using AutoMapper;
using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class SpedFiscalService : ISpedFiscalService
{
    private readonly ISpedFiscalRepository _repository;
    private readonly IMapper _mapper;

    public SpedFiscalService(ISpedFiscalRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<List<SpedFiscalGetDto>>> CreateManyAsync(List<SpedFiscalPostDto> dtos, string createdById)
    {
        var entities = dtos.Select(dto => _mapper.Map<SpedFiscal>(dto)).ToList();
        var created = await _repository.CreateManyAsync(entities, createdById);

        var result = created.Select(e => _mapper.Map<SpedFiscalGetDto>(e)).ToList();

        return new Response<List<SpedFiscalGetDto>>(
            true,
            $"{result.Count} registros de SPED Fiscal criados com sucesso",
            result,
            201
        );
    }

    public async Task<Response<List<SpedFiscalGetDto>>> GetByOportunidadeIdAsync(int oportunidadeId)
    {
        var entities = await _repository.GetByOportunidadeIdAsync(oportunidadeId);
        var dtos = entities.Select(e => _mapper.Map<SpedFiscalGetDto>(e)).ToList();

        if (!dtos.Any())
            return new Response<List<SpedFiscalGetDto>>(
                false,
                "Nenhum registro de SPED Fiscal encontrado para esta Oportunidade",
                404
            );

        return new Response<List<SpedFiscalGetDto>>(
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
