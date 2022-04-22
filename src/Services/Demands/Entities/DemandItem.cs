using Demands.Consts;

namespace Demands.Entities;

public class DemandItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DemandType Type { get; set; }

    public DemandState State { get; set; }

    public DateOnly ExpectAccomplishDate { get; set; }

    public DateOnly RealityAccomplishDate { get; set; }
}