using Eva.Demands.Const;

namespace Eva.Demands.Entities;

public class DemandsItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DemandType Type { get; set; }

    public DemandState State { get; set; }

    public DateOnly ExpectAccomplishDate { get; set; }

    public DateOnly RealityAccomplishDate { get; set; }
}