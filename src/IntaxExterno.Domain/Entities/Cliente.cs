using System;
using System.Collections.Generic;
using System.Text;

namespace IntaxExterno.Domain.Entities;

public class Cliente : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailResponsavel { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
}
