using FreeSql.DataAnnotations;

namespace Eva.Demands.Entities;

public class ModuleTask : BaseEntity
{
    public Guid DemandModuleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    [Column(MapType = typeof(int))]
    public TaskState State { get; set; }
}