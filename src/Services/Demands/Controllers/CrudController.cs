using AutoMapper;
using Eva.Demands.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Eva.Demands.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class CrudController<TEntity, TOutputDto, TCreateDto, TUpdateDto> : ControllerBase
    where TEntity : BaseEntity
    where TOutputDto : BasePrimaryKey
{
    private readonly IFreeSql _freeSql;
    private readonly IMapper _mapper;

    protected CrudController(IFreeSql freeSql, IMapper mapper)
    {
        _freeSql = freeSql;
        _mapper = mapper;
    }

    protected string UserId => User.Identity?.Name ?? "Anonymous";

    [HttpGet("{id}")]
    public virtual async Task<TOutputDto> GetByIdAsync(Guid id)
    {
        var repo = _freeSql.GetRepository<TEntity>();
        var entity = await repo.Select.FirstAsync(x => x.Id.Equals(id));
        return _mapper.Map<TOutputDto>(entity);
    }

    [HttpDelete("{id}")]
    public virtual async Task<bool> DeleteByIdAsync(Guid id)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();

        var entity = await repo.Select.Where(x => x.Id.Equals(id)).FirstAsync();

        entity.IsDeleted = true;
        entity.DeletedBy = UserId;
        entity.DeletedDate = DateTime.Now;

        var result = await repo.UpdateAsync(entity);

        uow.Commit();
        return result == 1;
    }

    [HttpPut("{id}")]
    public virtual async Task<TOutputDto> UpdateByIdAsync(Guid id, TUpdateDto dto)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = await repo.Select.Where(x => x.Id.Equals(id)).FirstAsync();
        _mapper.Map(dto, entity);

        entity.ModifiedBy = UserId;
        entity.ModifiedDate = DateTime.Now;

        uow.Commit();
        return _mapper.Map<TOutputDto>(entity);
    }

    [HttpPost]
    public virtual async Task<TOutputDto> CreateAsync(TCreateDto dto)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = _mapper.Map<TEntity>(dto);

        entity.CreatedBy = UserId;
        entity.CreatedDate = DateTime.Now;

        await repo.InsertAsync(entity);
        uow.Commit();
        return _mapper.Map<TOutputDto>(entity);
    }
}

public record BasePrimaryKey
{
    public Guid Id { get; set; }
}