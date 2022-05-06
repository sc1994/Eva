namespace Eva.Demands.Controllers.DemandsModules.Dto;

public record DemandsModuleCreateDto
{
    [Required] public Guid DemandsId { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public ModuleState State { get; set; }
}