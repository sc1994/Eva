using Eva.Demands.Const;

namespace Eva.Demands.Entities;

public class ModuleTask : BaseEntity
{
    public Guid DemandModuleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public TaskState State { get; set; }
}