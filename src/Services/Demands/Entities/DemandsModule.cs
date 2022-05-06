using FreeSql.DataAnnotations;

namespace Eva.Demands.Entities;

public class DemandsModule : BaseEntity
{
    public Guid DemandsId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Column(MapType = typeof(int))] public ModuleState State { get; set; }
}