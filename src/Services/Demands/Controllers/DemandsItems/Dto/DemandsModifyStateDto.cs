namespace Eva.Demands.Controllers.DemandsItems.Dto;

public record DemandsModifyStateDto([Required]Guid Id, [Required]DemandState State);