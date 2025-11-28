namespace IntaxExterno.Application.DTOs.Teses;

public class TesesGetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descrição { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
