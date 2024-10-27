namespace BuildingBlocks.Contracts;

public class AuditData
{
    public DateTime CreateDate { get; set; }
    public string CreateUser { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string  ModifiedUser { get; set; }
    public DateTime DeletedDate { get; set; }
    public string DeletedUser { get; set; }
}