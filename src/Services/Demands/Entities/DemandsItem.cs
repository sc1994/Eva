using FreeSql.DataAnnotations;

namespace Eva.Demands.Entities;

public class DemandsItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Column(MapType = typeof(int))]
    public DemandType Type { get; set; }

    [Column(MapType = typeof(int))]
    public DemandState State { get; set; }

    public DateTime ExpectAccomplishDate { get; set; }

    public DateTime RealityAccomplishDate { get; set; }
}