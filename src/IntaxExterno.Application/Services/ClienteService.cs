using AutoMapper;
using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<Response<ClientePostDto>> CreateAsync(ClientePostDto clientePostDto, string createdById)
    {
        Cliente? cliente = _mapper.Map<Cliente>(clientePostDto);
        Cliente? createdCliente = await _clienteRepository.CreateAsync(cliente, createdById);
        if (createdCliente == null)
            return new Response<ClientePostDto>(false, "Failed to create cliente", null, 500);
        return new Response<ClientePostDto>(true, "Success", _mapper.Map<ClientePostDto>(createdCliente), 201);
    }

    public async Task<Response<IEnumerable<ClienteGetDto>>> GetAllAsync()
    {
        IEnumerable<Cliente> clientes = await _clienteRepository.GetAllAsync();

        if (!clientes.Any())
            return new Response<IEnumerable<ClienteGetDto>>(false, "No clientes found", null, 404);

        return new Response<IEnumerable<ClienteGetDto>>(true, "Success", _mapper.Map<IEnumerable<ClienteGetDto>>(clientes), 200);
    }

    public async Task<Response<ClienteGetDetailsDto?>> GetByIdAsync(int id)
    {
        Cliente? cliente = await _clienteRepository.GetByIdAsync(id);
        if (cliente == null)
        {
            return new Response<ClienteGetDetailsDto?>(false, "Cliente not found", 404);
        }
        return new Response<ClienteGetDetailsDto?>(true, "Success", _mapper.Map<ClienteGetDetailsDto>(cliente), 200);
    }

    public async Task<Response<ClientePutDto>> UpdateAsync(ClientePutDto clientePutDto, string updatedById)
    {
        Cliente? cliente = _mapper.Map<Cliente>(clientePutDto);
        Cliente? updatedCliente = await _clienteRepository.UpdateAsync(cliente, updatedById);
        if (updatedCliente == null)
        {
            return new Response<ClientePutDto>(false, "Cliente not found", 404);
        }
        return new Response<ClientePutDto>(true, "Success", _mapper.Map<ClientePutDto>(updatedCliente), 200);
    }

    public async Task<Response<bool>> DeleteAsync(int id, int clienteId, string deletedById)
    {
        Cliente? cliente = await _clienteRepository.GetByIdAsync(id);
        if (cliente == null)
        {
            return new Response<bool>(false, "Cliente not found", 404);
        }
        if (cliente.Id != clienteId)
        {
            return new Response<bool>(false, "You are not allowed to delete this cliente", 403);
        }
        bool result = await _clienteRepository.DeleteAsync(id, deletedById);
        return new Response<bool>(result, "Success", 200);
    }
}
