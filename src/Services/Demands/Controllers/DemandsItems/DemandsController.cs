using Eva.Demands.Controllers.DemandsItems.Dto;

namespace Eva.Demands.Controllers.DemandsItems;

public class DemandsController : CrudController<DemandsItem, DemandsOutputDto, DemandsCreateDto, DemandsModifiedDto>
{
    private readonly IFreeSql _freeSql;

    public DemandsController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
        _freeSql = freeSql;
    }

    [HttpPut("ModifyState")]
    public async Task<bool> ModifyStateAsync(Guid id, DemandState state)
    {
        using var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<DemandsItem>();

        var entity = await repo.Select.Where(x => x.Id.Equals(id)).FirstAsync();
        if (entity == null) return false;

        entity.State = state;
        entity.ModifiedBy = UserName;
        entity.ModifiedDate = DateTime.Now;

        var result = await repo.UpdateAsync(entity);
        uow.Commit();

        return result == 1;
    }

    [HttpGet("now")]
    public string GetDatetimeNow()
    {
        return DateTime.Now.ToString("f");
    }
}