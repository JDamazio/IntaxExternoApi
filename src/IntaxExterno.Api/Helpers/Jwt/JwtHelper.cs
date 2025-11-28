using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntaxExterno.Api.Helpers.Jwt;

public static class JwtHelper
{
    public static string GetClaimValueFromToken(string token, string claimType)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

        return claim?.Value!;
    }

    public static string GetUserIdFromToken(string token)
    {
        return GetClaimValueFromToken(token, JwtRegisteredClaimNames.Sub);
    }

    public static string GetUserRoleFromToken(string token)
    {
        return GetClaimValueFromToken(token, ClaimTypes.Role);
    }

    public static string GetUserEmailFromToken(string token)
    {
        return GetClaimValueFromToken(token, ClaimTypes.Email);
    }
}
