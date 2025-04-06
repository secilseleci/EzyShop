using Models.Entities.Abstract;

public abstract class BaseEntity : IBaseEntity, IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

    public Guid? CreatedById { get; set; }
    public Guid? ModifiedById { get; set; }
    public Guid? DeletedById { get; set; }
 
}
