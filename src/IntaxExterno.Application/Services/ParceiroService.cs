using AutoMapper;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class ParceiroService : IParceiroService
{
    private readonly IParceiroRepository _parceiroRepository;
    private readonly IMapper _mapper;

    public ParceiroService(IParceiroRepository parceiroRepository, IMapper mapper)
    {
        _parceiroRepository = parceiroRepository;
        _mapper = mapper;
    }

    public async Task<Response<ParceiroPostDto>> CreateAsync(ParceiroPostDto parceiroPostDto, string createdById)
    {
        Parceiro? parceiro = _mapper.Map<Parceiro>(parceiroPostDto);
        Parceiro? createdParceiro = await _parceiroRepository.CreateAsync(parceiro, createdById);
        if (createdParceiro == null)
            return new Response<ParceiroPostDto>(false, "Failed to create parceiro", null, 500);
        return new Response<ParceiroPostDto>(true, "Success", _mapper.Map<ParceiroPostDto>(createdParceiro), 201);
    }

    public async Task<Response<IEnumerable<ParceiroGetDto>>> GetAllAsync()
    {
        IEnumerable<Parceiro> parceiros = await _parceiroRepository.GetAllAsync();

        if (!parceiros.Any())
            return new Response<IEnumerable<ParceiroGetDto>>(false, "No parceiros found", null, 404);

        return new Response<IEnumerable<ParceiroGetDto>>(true, "Success", _mapper.Map<IEnumerable<ParceiroGetDto>>(parceiros), 200);
    }

    public async Task<Response<ParceiroGetDetailsDto?>> GetByIdAsync(int id)
    {
        Parceiro? parceiro = await _parceiroRepository.GetByIdAsync(id);
        if (parceiro == null)
        {
            return new Response<ParceiroGetDetailsDto?>(false, "Parceiro not found", 404);
        }
        return new Response<ParceiroGetDetailsDto?>(true, "Success", _mapper.Map<ParceiroGetDetailsDto>(parceiro), 200);
    }

    public async Task<Response<ParceiroPutDto>> UpdateAsync(ParceiroPutDto parceiroPutDto, string updatedById)
    {
        Parceiro? parceiro = _mapper.Map<Parceiro>(parceiroPutDto);
        Parceiro? updatedParceiro = await _parceiroRepository.UpdateAsync(parceiro, updatedById);
        if (updatedParceiro == null)
        {
            return new Response<ParceiroPutDto>(false, "Parceiro not found", 404);
        }
        return new Response<ParceiroPutDto>(true, "Success", _mapper.Map<ParceiroPutDto>(updatedParceiro), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int parceiroId, string deletedById)
    {
        Parceiro? parceiro = await _parceiroRepository.GetByIdAsync(id);
        if (parceiro == null)
        {
            return new Response<bool>(false, "Parceiro not found", 404);
        }
        if (parceiro.Id != parceiroId)
        {
            return new Response<bool>(false, "You are not allowed to delete this parceiro", 403);
        }
        bool result = await _parceiroRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }
}
