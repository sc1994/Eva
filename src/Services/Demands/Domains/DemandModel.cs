using Demands.Consts;

namespace Demands.Domains;

public class DemandModel
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DemandType Type { get; set; }

    public DemandState State { get; set; }

    public DateOnly ExpectAccomplishDate { get; set; }

    public DateOnly RealityAccomplishDate { get; set; }

    public IEnumerable<DemandModuleModel> Modules { get; set; } = new List<DemandModuleModel>();
}