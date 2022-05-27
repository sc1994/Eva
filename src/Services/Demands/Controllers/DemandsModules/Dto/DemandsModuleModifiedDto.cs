namespace Eva.Demands.Controllers.DemandsModules.Dto;

public record DemandsModuleModifiedDto
{
    public Guid DemandsId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ModuleState State { get; set; }
}