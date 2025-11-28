namespace IntaxExterno.Api.Models;

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
