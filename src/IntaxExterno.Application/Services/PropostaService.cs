using AutoMapper;
using IntaxExterno.Application.DTOs.Proposta;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class PropostaService : IPropostaService
{
    private readonly IPropostaRepository _propostaRepository;
    private readonly IPropostaTesesRepository _propostaTesesRepository;
    private readonly IMapper _mapper;

    public PropostaService(IPropostaRepository propostaRepository, IPropostaTesesRepository propostaTesesRepository, IMapper mapper)
    {
        _propostaRepository = propostaRepository;
        _propostaTesesRepository = propostaTesesRepository;
        _mapper = mapper;
    }

    public async Task<Response<PropostaPostDto>> CreateAsync(PropostaPostDto propostaPostDto, string createdById)
    {
        Proposta? proposta = _mapper.Map<Proposta>(propostaPostDto);
        Proposta? createdProposta = await _propostaRepository.CreateAsync(proposta, createdById);
        if (createdProposta == null)
            return new Response<PropostaPostDto>(false, "Failed to create proposta", null, 500);

        // Criar registros na tabela PropostaTeses
        if (propostaPostDto.TesesIds != null && propostaPostDto.TesesIds.Any())
        {
            foreach (var tesesId in propostaPostDto.TesesIds)
            {
                var propostaTeses = new PropostaTeses
                {
                    PropostaId = createdProposta.Id,
                    TesesId = tesesId
                };
                await _propostaTesesRepository.CreateAsync(propostaTeses, createdById);
            }
        }

        return new Response<PropostaPostDto>(true, "Success", _mapper.Map<PropostaPostDto>(createdProposta), 201);
    }

    public async Task<Response<IEnumerable<PropostaGetDto>>> GetAllAsync()
    {
        IEnumerable<Proposta> propostas = await _propostaRepository.GetAllAsync();

        if (!propostas.Any())
            return new Response<IEnumerable<PropostaGetDto>>(false, "No propostas found", null, 404);

        return new Response<IEnumerable<PropostaGetDto>>(true, "Success", _mapper.Map<IEnumerable<PropostaGetDto>>(propostas), 200);
    }

    public async Task<Response<PropostaGetDetailsDto?>> GetByIdAsync(int id)
    {
        Proposta? proposta = await _propostaRepository.GetByIdAsync(id);
        if (proposta == null)
        {
            return new Response<PropostaGetDetailsDto?>(false, "Proposta not found", 404);
        }
        return new Response<PropostaGetDetailsDto?>(true, "Success", _mapper.Map<PropostaGetDetailsDto>(proposta), 200);
    }

    public async Task<Response<PropostaPutDto>> UpdateAsync(PropostaPutDto propostaPutDto, string updatedById)
    {
        Proposta? proposta = _mapper.Map<Proposta>(propostaPutDto);
        Proposta? updatedProposta = await _propostaRepository.UpdateAsync(proposta, updatedById);
        if (updatedProposta == null)
        {
            return new Response<PropostaPutDto>(false, "Proposta not found", 404);
        }

        // Atualizar registros na tabela PropostaTeses
        // Primeiro remove os antigos (soft delete)
        await _propostaTesesRepository.DeleteByPropostaIdAsync(updatedProposta.Id, updatedById);

        // Depois cria os novos
        if (propostaPutDto.TesesIds != null && propostaPutDto.TesesIds.Any())
        {
            foreach (var tesesId in propostaPutDto.TesesIds)
            {
                var propostaTeses = new PropostaTeses
                {
                    PropostaId = updatedProposta.Id,
                    TesesId = tesesId
                };
                await _propostaTesesRepository.CreateAsync(propostaTeses, updatedById);
            }
        }

        return new Response<PropostaPutDto>(true, "Success", _mapper.Map<PropostaPutDto>(updatedProposta), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int propostaId, string deletedById)
    {
        Proposta? proposta = await _propostaRepository.GetByIdAsync(id);
        if (proposta == null)
        {
            return new Response<bool>(false, "Proposta not found", 404);
        }
        if (proposta.Id != propostaId)
        {
            return new Response<bool>(false, "You are not allowed to delete this proposta", 403);
        }
        bool result = await _propostaRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }
}
