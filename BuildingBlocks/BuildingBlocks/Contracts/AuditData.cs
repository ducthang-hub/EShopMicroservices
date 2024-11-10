namespace BuildingBlocks.Contracts;

public class AuditData
{
    public DateTime CreateDate { get; set; }
    public string CreateUser { get; set; } = string.Empty;
    public DateTime? ModifiedDate { get; set; }
    public string?  ModifiedUser { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? DeletedUser { get; set; }

    public void PopulateAudit(string userId, bool isModified = false)
    {
        if (isModified)
        {
            ModifiedDate = DateTime.UtcNow;
            ModifiedUser = userId;
        }
        else
        {
            CreateDate = DateTime.UtcNow;
            CreateUser = userId;
        }
    }
}