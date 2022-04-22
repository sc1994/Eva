using Demands.Consts;

namespace Demands.Entities;

public class DemandModule : BaseEntity
{
    public Guid DemandItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = String.Empty;

    public ModuleState State { get; set; }
}