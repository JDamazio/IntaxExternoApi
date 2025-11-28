using System.ComponentModel.DataAnnotations;

namespace IntaxExterno.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public string UID { get; set; } = default!; // TODO: J. Depois criar o EntityTypeBuilder

    [Required]
    public DateTime Created { get; protected set; }

    [MaxLength(50)]
    public string? CreatedBy { get; protected set; }

    [MaxLength(50)]
    public string? UpdatedBy { get; protected set; }

    [MaxLength(50)]
    public string? DeletedBy { get; protected set; }

    public bool IsActive { get; protected set; }

    public DateTime? Updated { get; protected set; }

    public DateTime? Deleted { get; protected set; }

    public void Create(string createdById)
    {
        UID = Guid.NewGuid().ToString();
        CreatedBy = createdById;
        IsActive = true;
        Created = DateTime.Now.ToUniversalTime();
    }

    public void Update(string updatedById)
    {
        UpdatedBy = updatedById;
        Updated = DateTime.Now.ToUniversalTime();
    }

    public void Delete(string deletedById)
    {
        DeletedBy = deletedById;
        Deleted = DateTime.Now.ToUniversalTime();
        IsActive = false;
    }
}
