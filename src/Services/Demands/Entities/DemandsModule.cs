using Eva.Demands.Const;

namespace Eva.Demands.Entities;

public class DemandsModule : BaseEntity
{
    public Guid DemandItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = String.Empty;

    public ModuleState State { get; set; }
}