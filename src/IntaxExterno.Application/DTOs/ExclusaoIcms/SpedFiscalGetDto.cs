namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class SpedFiscalGetDto
{
    public int Id { get; set; }
    public int OportunidadeId { get; set; }
    public string Cfop { get; set; } = string.Empty;
    public string CstIcms { get; set; } = string.Empty;
    public decimal? ValorIcms { get; set; }
    public DateTime? DataInicial { get; set; }
    public bool IsActive { get; set; }
}
