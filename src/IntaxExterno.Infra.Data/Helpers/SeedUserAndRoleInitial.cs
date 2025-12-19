using IntaxExterno.Domain.Enums;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Infra.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace IntaxExterno.Infra.Data.Helpers;

/// <summary>
/// Implementação de seed de usuários e roles iniciais
/// </summary>
public class SeedUserAndRoleInitial : ISeedUserAndRoleInitial
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public SeedUserAndRoleInitial(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Cria todas as roles do sistema (Admin, Cliente)
    /// </summary>
    public void SeedRoles()
    {
        var roles = RoleTypeExtensions.GetAllRoles().OrderBy(x => x);

        foreach (var roleName in roles)
        {
            if (!_roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole role = new()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                IdentityResult result = _roleManager.CreateAsync(role).Result;
            }
        }
    }

    /// <summary>
    /// Cria usuários de teste para cada role
    /// Admin: test.admin@intaxexterno.com (senha: Test@123)
    /// Cliente: test.cliente@intaxexterno.com (senha: Test@123)
    /// </summary>
    public void SeedUsers()
    {
        var roles = RoleTypeExtensions.GetAllRoles().OrderBy(x => x);

        foreach (var roleName in roles)
        {
            var email = $"test.{roleName.ToLower()}@intaxexterno.com";

            if (_userManager.FindByEmailAsync(email).Result == null)
            {
                User user = new()
                {
                    Name = $"Test {roleName}",
                    UserName = email,
                    Email = email,
                    NormalizedUserName = email.ToUpper(),
                    NormalizedEmail = email.ToUpper(),
                    EmailConfirmed = true,
                    IsActive = true,
                    Created = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                IdentityResult result = _userManager.CreateAsync(user, "Test@123").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, roleName).Wait();
                }
            }
        }
    }
}
