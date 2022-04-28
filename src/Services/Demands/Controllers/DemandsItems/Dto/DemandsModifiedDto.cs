namespace Eva.Demands.Controllers.DemandsItems.Dto;

[MapTo(typeof(DemandsItem))]
public record DemandsModifiedDto
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public DemandType Type { get; set; }

    [Required] public DemandState State { get; set; }

    [Required] public DateTime ExpectAccomplishDate { get; set; }

    [Required] public DateTime RealityAccomplishDate { get; set; }
}