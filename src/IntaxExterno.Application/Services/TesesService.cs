using AutoMapper;
using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class TesesService : ITesesService
{
    private readonly ITesesRepository _tesesRepository;
    private readonly IMapper _mapper;

    public TesesService(ITesesRepository tesesRepository, IMapper mapper)
    {
        _tesesRepository = tesesRepository;
        _mapper = mapper;
    }

    public async Task<Response<TesesPostDto>> CreateAsync(TesesPostDto tesesPostDto, string createdById)
    {
        Teses? teses = _mapper.Map<Teses>(tesesPostDto);
        Teses? createdTeses = await _tesesRepository.CreateAsync(teses, createdById);
        if (createdTeses == null)
            return new Response<TesesPostDto>(false, "Failed to create teses", null, 500);
        return new Response<TesesPostDto>(true, "Success", _mapper.Map<TesesPostDto>(createdTeses), 201);
    }

    public async Task<Response<IEnumerable<TesesGetDto>>> GetAllAsync()
    {
        IEnumerable<Teses> teses = await _tesesRepository.GetAllAsync();

        if (!teses.Any())
            return new Response<IEnumerable<TesesGetDto>>(false, "No teses found", null, 404);

        return new Response<IEnumerable<TesesGetDto>>(true, "Success", _mapper.Map<IEnumerable<TesesGetDto>>(teses), 200);
    }

    public async Task<Response<TesesGetDetailsDto?>> GetByIdAsync(int id)
    {
        Teses? teses = await _tesesRepository.GetByIdAsync(id);
        if (teses == null)
        {
            return new Response<TesesGetDetailsDto?>(false, "Teses not found", 404);
        }
        return new Response<TesesGetDetailsDto?>(true, "Success", _mapper.Map<TesesGetDetailsDto>(teses), 200);
    }

    public async Task<Response<TesesPutDto>> UpdateAsync(TesesPutDto tesesPutDto, string updatedById)
    {
        Teses? teses = _mapper.Map<Teses>(tesesPutDto);
        Teses? updatedTeses = await _tesesRepository.UpdateAsync(teses, updatedById);
        if (updatedTeses == null)
        {
            return new Response<TesesPutDto>(false, "Teses not found", 404);
        }
        return new Response<TesesPutDto>(true, "Success", _mapper.Map<TesesPutDto>(updatedTeses), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int tesesId, string deletedById)
    {
        Teses? teses = await _tesesRepository.GetByIdAsync(id);
        if (teses == null)
        {
            return new Response<bool>(false, "Teses not found", 404);
        }
        if (teses.Id != tesesId)
        {
            return new Response<bool>(false, "You are not allowed to delete this teses", 403);
        }
        bool result = await _tesesRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }
}
