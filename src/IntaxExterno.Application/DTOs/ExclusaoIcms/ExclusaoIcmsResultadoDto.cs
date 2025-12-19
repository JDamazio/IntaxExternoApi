namespace IntaxExterno.Application.DTOs.ExclusaoIcms;

public class ExclusaoIcmsResultadoDto
{
    public DateTime? DataInicial { get; set; }
    public decimal? ValorIcms { get; set; }
    public decimal? ValorPis { get; set; }
    public decimal? ValorCofins { get; set; }
    public decimal? ValorPisCofins { get; set; }
}
