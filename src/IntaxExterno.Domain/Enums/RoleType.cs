using System.ComponentModel;

namespace IntaxExterno.Domain.Enums;

/// <summary>
/// Tipos de roles do sistema Intax Externo
/// </summary>
public enum RoleType
{
    [Description("Admin")]
    Admin,

    [Description("Cliente")]
    Cliente
}
