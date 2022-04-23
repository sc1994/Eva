using AutoMapper;
using Eva.Demands.Controllers.DemandsItems.Dto;
using Eva.Demands.Entities;

namespace Eva.Demands.Controllers.DemandsItems;

public class DemandsController : CrudController<DemandsItem, DemandsOutputDto, DemandsCreateDto, DemandsUpdateDto>
{
    public DemandsController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}