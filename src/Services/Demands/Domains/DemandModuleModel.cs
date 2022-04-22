using Demands.Consts;

namespace Demands.Domains;

public class DemandModuleModel : BaseModel
{
    public Guid DemandItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = String.Empty;

    public ModuleState State { get; set; }

    public IEnumerable<ModuleTaskModel> Tasks { get; set; } = new List<ModuleTaskModel>();
}