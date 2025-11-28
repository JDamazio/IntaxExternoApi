namespace IntaxExterno.Domain.Entities;

public class PropostaTeses : BaseEntity
{
    public int PropostaId { get; set; }
    public Proposta Proposta { get; set; } = default!;

    public int TesesId { get; set; }
    public Teses Teses { get; set; } = default!;
}
