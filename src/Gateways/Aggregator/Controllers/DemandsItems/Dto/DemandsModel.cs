using Eva.Demands.Domains;

namespace Eva.HttpAggregator.Controllers.DemandsItems.Dto;

public class DemandsModel
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DemandType Type { get; set; }

    public DemandState State { get; set; }

    public DateOnly ExpectAccomplishDate { get; set; }

    public DateOnly RealityAccomplishDate { get; set; }

    public IEnumerable<DemandsModuleModel> Modules { get; set; } = new List<DemandsModuleModel>();
}