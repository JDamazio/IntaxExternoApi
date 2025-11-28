using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntaxExterno.Api.Helpers.Auth;
using IntaxExterno.Api.Models;
using IntaxExterno.Infra.Data.Identity;

namespace IntaxExterno.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly Auth _auth;

    public AuthController(UserManager<User> userManager, Auth auth)
    {
        _userManager = userManager;
        _auth = auth;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserToken>> Login(UserLogin userLogin)
    {
        User user = await _userManager.FindByEmailAsync(userLogin.Email);

        if (user is null)
            return StatusCode(204, "User not found");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
        if (!isPasswordValid)
            return StatusCode(204, "Please check the provided password");

        var result = await _auth.GenerateToken(userLogin);

        if (result is null)
            return StatusCode(204, "Failed to generate token");

        return Ok(result);
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserToken>> Register(UserRegister userRegister)
    {
        var existing = await _userManager.FindByEmailAsync(userRegister.Email);
        if (existing is not null)
            return Conflict("Email already in use.");

        var user = new User
        {
            UserName = userRegister.Email,
            Email = userRegister.Email,
            Name = userRegister.Email.Split('@')[0]
        };

        var createResult = await _userManager.CreateAsync(user, userRegister.Password);
        if (!createResult.Succeeded)
            return BadRequest(createResult.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, userRegister.Role);

        return CreatedAtAction(nameof(Login), new { email = userRegister.Email });
    }

    [HttpGet("Users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUsers()
    {
        var users = await _userManager.Users
            .Where(u => u.Deleted == null)
            .OrderBy(u => u.Name)
            .ToListAsync();

        var userInfoList = new List<UserInfo>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            userInfoList.Add(new UserInfo
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive,
                Created = user.Created,
                Roles = roles
            });
        }

        return Ok(userInfoList);
    }
}
