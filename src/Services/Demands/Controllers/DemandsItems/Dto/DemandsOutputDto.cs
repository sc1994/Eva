namespace Eva.Demands.Controllers.DemandsItems.Dto;

[MapTo(typeof(DemandsItem))]
public record DemandsOutputDto : BaseOutputDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DemandType Type { get; set; }

    public DemandState State { get; set; }

    public DateTime ExpectAccomplishDate { get; set; }

    public DateTime RealityAccomplishDate { get; set; }
}