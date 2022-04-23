using Eva.Demands.Entities;
using Eva.ToolKit;

namespace Eva.Demands.Controllers.DemandsItems.Dto;

[MapTo(typeof(DemandsItem))]
public record DemandsUpdateDto(string Name);