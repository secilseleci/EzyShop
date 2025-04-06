namespace Models.Entities.Abstract;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
    bool IsDeleted { get; set; }

    Guid? CreatedById { get; set; }
    Guid? ModifiedById { get; set; }
    Guid? DeletedById { get; set; }

}
