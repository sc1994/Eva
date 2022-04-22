namespace Demands.Domains;

public class BaseModel
{
    public Guid Id { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string ModifiedBy { get; set; } = string.Empty;

    public DateTime CreatedTime { get; set; }

    public DateTime ModifiedTime { get; set; }

    public bool IsDeleted { get; set; }
}