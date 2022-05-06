namespace Eva.ToolKit;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string ModifiedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string DeletedBy { get; set; } = string.Empty;

    public DateTime DeletedDate { get; set; }
}