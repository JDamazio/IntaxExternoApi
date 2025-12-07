using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IntaxExterno.Domain.Entities;

public class RelatorioDeCreditoPerse : BaseEntity
{
    public DateTime DataEmissao { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalIRPJ { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCSLL { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPIS { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCOFINS { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Saldo { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = default!;

    public List<ItemRelatorioDeCreditoPerse> Itens { get; set; } = new List<ItemRelatorioDeCreditoPerse>();
}
