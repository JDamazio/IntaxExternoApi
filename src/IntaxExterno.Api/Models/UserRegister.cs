using System.ComponentModel.DataAnnotations;

namespace IntaxExterno.Api.Models;

public class UserRegister
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(6)]
    public string Password { get; set; } = default!;

    [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = default!;

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = default!;
}
