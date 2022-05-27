using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Eva.ToolKit.Attributes;
using FreeSql;
using Microsoft.AspNetCore.Mvc;

namespace Eva.ToolKit;

public abstract class CrudController<TEntity, TOutputDto, TCreateOrModifiedDto> : CrudController<TEntity, TOutputDto, TCreateOrModifiedDto, TCreateOrModifiedDto>
    where TEntity : BaseEntity
    where TOutputDto : BasePrimaryKey
{
    protected CrudController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}

[Route("/[controller]")]
[ApiController]
[FormatResponse]
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
        var entity = await repo.Select.Where(x => x.Id.Equals(id)).FirstAsync();

        return _mapper.Map<TOutputDto>(entity ?? throw new NullReferenceException(nameof(entity)));
    }

    [HttpDelete("{id}")]
    public virtual async Task<bool> DeleteByIdAsync([Required] Guid id)
    {
        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();

        var result = await repo.DeleteAsync(x => x.Id.Equals(id));

        uow.Commit();
        return result == 1;
    }

    [HttpPut("{id}")]
    public virtual async Task<TOutputDto> ModifyByIdAsync([Required] Guid id, TModifiedDto input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = await repo.Select.Where(x => x.Id.Equals(id)).FirstAsync();
        if (entity == null) throw new NullReferenceException(nameof(entity));

        _mapper.Map(input, entity);
        entity.ModifiedBy = UserName;
        entity.ModifiedDate = DateTime.Now;

        await repo.UpdateAsync(entity);

        uow.Commit();
        return await GetByIdAsync(entity.Id);
    }

    [HttpPost]
    public virtual async Task<TOutputDto> CreateAsync(TCreateDto input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var uow = _freeSql.CreateUnitOfWork();
        var repo = uow.GetRepository<TEntity>();
        var entity = _mapper.Map<TEntity>(input);

        entity.CreatedBy = UserName;
        entity.CreatedDate = DateTime.Now;

        await repo.InsertAsync(entity);
        uow.Commit();
        return await GetByIdAsync(entity.Id);
    }
}

public abstract class QueryController<TEntity, TOutputDto, TCreateOrModifiedDto, TQueryDto> : QueryController<TEntity, TOutputDto, TCreateOrModifiedDto, TCreateOrModifiedDto, TQueryDto>
    where TEntity : BaseEntity
    where TOutputDto : BasePrimaryKey
{
    protected QueryController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}

public abstract class QueryController<TEntity, TOutputDto, TCreateDto, TModifiedDto, TQueryDto> : CrudController<TEntity, TOutputDto, TCreateDto, TModifiedDto>
    where TEntity : BaseEntity
    where TOutputDto : BasePrimaryKey
{
    private readonly IMapper _mapper;

    protected QueryController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
        _mapper = mapper;
    }

    protected abstract ISelect<TEntity> QueryFilter(TQueryDto input);

    [HttpGet("query")]
    [HttpPost("query")]
    public async Task<IEnumerable<TOutputDto>> QueryAsync(TQueryDto input)
    {
        var list = await QueryFilter(input).ToListAsync();

        if (list?.Any() != true)
        {
            return Array.Empty<TOutputDto>();
        }

        return _mapper.Map<IEnumerable<TOutputDto>>(list);
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
}