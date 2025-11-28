using System.ComponentModel.DataAnnotations;

namespace IntaxExterno.Api.Models;

public class UserLogin
{
    [Required(ErrorMessage = "Informe o Email")]
    [EmailAddress(ErrorMessage = "Formato do email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a Senha")]
    public string Password { get; set; } = string.Empty;
}
