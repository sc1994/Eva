using Eva.Demands.Controllers.DemandsModules.Dto;

namespace Eva.Demands.Controllers.DemandsModules;

public class DemandsModuleController : CrudController<DemandsModule, DemandsModuleOutputDto, DemandsModuleCreateDto, DemandsModuleModifiedDto>
{
    private readonly IFreeSql _freeSql;

    public DemandsModuleController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
        _freeSql = freeSql;
    }

    [HttpPut("ModifyState")]
    public async Task<bool> ModifyStateAsync(DemandsModuleModifyStateDto input)
    {
        using var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<DemandsModule>();

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