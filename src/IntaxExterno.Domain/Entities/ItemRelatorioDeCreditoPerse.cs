using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IntaxExterno.Domain.Entities;

public class ItemRelatorioDeCreditoPerse : BaseEntity
{
    public string TipoTributo { get; set; } = string.Empty; // IRPJ, CSLL, PIS, COFINS
    public DateTime DataEmissao { get; set; }

    public int? NumPedido { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSolicitado { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CorrecaoMonetaria { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalRecebido { get; set; }

    public string? Observacao { get; set; }

    public int RelatorioDeCreditoPerseId { get; set; }
    public RelatorioDeCreditoPerse RelatorioDeCreditoPerse { get; set; } = default!;
}
