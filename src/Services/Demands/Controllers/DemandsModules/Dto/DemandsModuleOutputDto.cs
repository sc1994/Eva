namespace Eva.Demands.Controllers.DemandsModules.Dto;

public record DemandsModuleOutputDto : BaseOutputDto
{
    public Guid DemandsId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = String.Empty;

    public ModuleState State { get; set; }
}