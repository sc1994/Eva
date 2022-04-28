﻿using System.Diagnostics.CodeAnalysis;

namespace Eva.Demands.Controllers;

[Route("/[controller]")]
[ApiController]
public abstract class CrudController<TEntity, TOutputDto, TCreateDto, TModifiedDto> : ControllerBase
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

    protected string UserName => User.Identity?.Name ?? "Anonymous";

    [HttpGet("{id}")]
    public virtual async Task<TOutputDto> GetByIdAsync([Required] Guid id)
    {
        var repo = _freeSql.GetRepository<TEntity>();
        var entity = await repo.Select.Where(x => !x.IsDeleted).Where(x => x.Id.Equals(id)).FirstAsync();

        return _mapper.Map<TOutputDto>(entity ?? throw new NullReferenceException(nameof(entity)));
    }

    [HttpDelete("{id}")]
    public virtual async Task<bool> DeleteByIdAsync([Required] Guid id)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();

        var entity = await repo.Select.Where(x => !x.IsDeleted).Where(x => x.Id.Equals(id)).FirstAsync();
        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;
        entity.DeletedBy = UserName;
        entity.DeletedDate = DateTime.Now;

        var result = await repo.UpdateAsync(entity);

        uow.Commit();
        return result == 1;
    }

    [HttpPut("{id}")]
    public virtual async Task<TOutputDto> ModifyByIdAsync([Required] Guid id, [Required] [DisallowNull] TModifiedDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = await repo.Select.Where(x => !x.IsDeleted).Where(x => x.Id.Equals(id)).FirstAsync();
        if (entity == null)
        {
            throw new NullReferenceException(nameof(entity));
        }

        _mapper.Map(dto, entity);
        entity.ModifiedBy = UserName;
        entity.ModifiedDate = DateTime.Now;

        await repo.UpdateAsync(entity);

        uow.Commit();
        return _mapper.Map<TOutputDto>(entity);
    }

    [HttpPost]
    public virtual async Task<TOutputDto> CreateAsync([Required] [DisallowNull] TCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = _mapper.Map<TEntity>(dto);

        entity.CreatedBy = UserName;
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

public record BaseOutputDto : BasePrimaryKey
{
    public string CreatedBy { get; set; } = string.Empty;

    public string ModifiedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public string DeletedBy { get; set; } = string.Empty;

    public DateTime DeletedDate { get; set; }
}