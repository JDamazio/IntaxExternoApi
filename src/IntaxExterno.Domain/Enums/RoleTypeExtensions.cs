namespace IntaxExterno.Domain.Enums;

/// <summary>
/// Extensões para trabalhar com RoleType
/// </summary>
public static class RoleTypeExtensions
{
    /// <summary>
    /// Converte RoleType enum para string
    /// </summary>
    public static string ToRoleString(this RoleType roleType)
    {
        return roleType.ToString();
    }

    /// <summary>
    /// Converte uma string para RoleType enum
    /// </summary>
    public static RoleType ToRoleType(this string roleString)
    {
        return Enum.Parse<RoleType>(roleString, ignoreCase: true);
    }

    /// <summary>
    /// Tenta converter uma string para RoleType enum
    /// </summary>
    public static bool TryParseRoleType(this string roleString, out RoleType roleType)
    {
        return Enum.TryParse(roleString, ignoreCase: true, out roleType);
    }

    /// <summary>
    /// Retorna todas as roles disponíveis como lista de strings
    /// Usado para seeding
    /// </summary>
    public static List<string> GetAllRoles()
    {
        return Enum.GetValues<RoleType>()
            .Select(r => r.ToString())
            .ToList();
    }

    /// <summary>
    /// Retorna todas as roles disponíveis como lista de RoleType
    /// </summary>
    public static List<RoleType> GetAllRoleTypes()
    {
        return Enum.GetValues<RoleType>().ToList();
    }
}
