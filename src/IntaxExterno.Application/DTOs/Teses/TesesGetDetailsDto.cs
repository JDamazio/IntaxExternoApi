namespace IntaxExterno.Application.DTOs.Teses;

public class TesesGetDetailsDto
{
    public int Id { get; set; }
    public string UID { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descrição { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
}
