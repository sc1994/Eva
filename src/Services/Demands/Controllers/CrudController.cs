using Demands.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demands.Controllers;

public class CrudController<TEntity, TOutputDto, TCreateDto, TUpdateDto> : ControllerBase
    where TEntity : BaseEntity
{
    private readonly IFreeSql _freeSql;

    public CrudController(IFreeSql freeSql)
    {
        _freeSql = freeSql;
    }

    public virtual async Task<TOutputDto> GetById(Guid id)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();

        var entity = await repo.Select.FirstAsync(x => x.Id.Equals(id));

        return entity.
    }
}