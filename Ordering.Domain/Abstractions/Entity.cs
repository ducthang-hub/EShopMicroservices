namespace Ordering.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }

    public void PopulateAudit(string userId, bool isModified = false)
    {
        if (isModified)
        {
            ModifiedBy = userId;
            ModifiedDate = DateTime.UtcNow;
        }
        else
        {
            CreatedBy = userId;
            CreatedDate = DateTime.UtcNow;
        }
    }
}