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
    public async Task<bool> ModifyStateAsync(DemandsModifyStateDto input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        using var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<DemandsItem>();

        var entity = await repo.Select.Where(x => !x.IsDeleted).Where(x => x.Id.Equals(input.Id)).FirstAsync();
        if (entity == null)
        {
            return false;
        }

        entity.State = input.State;
        entity.ModifiedBy = UserName;
        entity.ModifiedDate = DateTime.Now;

        var result = await repo.UpdateAsync(entity);
        uow.Commit();

        return result == 1;
    }
}