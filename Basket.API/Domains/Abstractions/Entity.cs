namespace Basket.API.Domains.Abstractions;

public class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime? DeletedDate { get; set; }
    public string DeletedBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public void PopulateAudit(string byUser, bool isModified = false)
    {
        var currentDate = DateTime.UtcNow;
        if (isModified)
        {
            ModifiedDate = currentDate;
            ModifiedBy = byUser;
        }
        else
        {
            CreatedDate = currentDate;
            CreatedBy = byUser;
        }
    }

    public void Delete(string byUser = default!)
    {
        DeletedBy = byUser;
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;
    }
}