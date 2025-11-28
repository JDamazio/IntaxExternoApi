namespace IntaxExterno.Application.DTOs.Cliente;

public class ClientePutDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
}
