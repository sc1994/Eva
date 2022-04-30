namespace Eva.Demands.Controllers.DemandsModules.Dto;

public record DemandsModuleModifyStateDto([Required] Guid Id, [Required] ModuleState State);