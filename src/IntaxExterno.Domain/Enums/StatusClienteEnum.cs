using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IntaxExterno.Domain.Enums;

public enum StatusClienteEnum
{
    [Description("Em Negociação")]
    Analise = 0,
    [Description("Convertido")]
    Convertido = 1,
    [Description("Não Convertido")]
    NaoConvertido = 2,
}
