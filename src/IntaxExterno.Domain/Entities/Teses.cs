using System;
using System.Collections.Generic;
using System.Text;

namespace IntaxExterno.Domain.Entities;

public class Teses : BaseEntity
{
    public string Nome { get; set; }
    public string Descrição { get; set; }
}
