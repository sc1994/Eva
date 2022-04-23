using Eva.Demands.Entities;
using Eva.ToolKit;

namespace Eva.Demands.Controllers.DemandsItems.Dto;

[MapTo(typeof(DemandsItem))]
public record DemandsOutputDto : BasePrimaryKey
{
    public string Name { get; init; } = string.Empty;
}