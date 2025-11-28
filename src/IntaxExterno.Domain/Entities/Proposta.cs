using System;
using System.Collections.Generic;
using System.Text;

namespace IntaxExterno.Domain.Entities;

public class Proposta : BaseEntity
{
    public List<PropostaTeses> PropostaTeses { get; set; } = new List<PropostaTeses>();

    public int? ParceiroId { get; set; }
    public Parceiro Parceiro { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
}
