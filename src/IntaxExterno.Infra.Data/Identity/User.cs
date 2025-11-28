using Microsoft.AspNetCore.Identity;

namespace IntaxExterno.Infra.Data.Identity;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Updated { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
