namespace IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;

public class RelatorioImportResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int RelatoriosImportados { get; set; }
    public int ItensImportados { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}
