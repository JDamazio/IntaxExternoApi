using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Domain.Responses;

namespace IntaxExterno.Application.Interfaces;

public interface IClienteService
{
    Task<Response<ClientePostDto>> CreateAsync(ClientePostDto clientePostDto, string createdById);
    Task<Response<IEnumerable<ClienteGetDto>>> GetAllAsync();
    Task<Response<ClienteGetDetailsDto?>> GetByIdAsync(int id);
    Task<Response<ClientePutDto>> UpdateAsync(ClientePutDto clientePutDto, string updatedById);
    Task<Response<bool>> DeleteAsync(int id, int clienteId, string deletedById);
}
