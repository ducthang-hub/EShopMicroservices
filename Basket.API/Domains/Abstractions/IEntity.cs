namespace Basket.API.Domains.Abstractions;

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}