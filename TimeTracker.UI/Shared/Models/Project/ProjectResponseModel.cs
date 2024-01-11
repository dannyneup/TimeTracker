using TimeTracker.UI.Windows.Shared.Interfaces;

namespace TimeTracker.UI.Windows.Shared.Models.Project;

public record ProjectResponseModel(int Id, string Name, string Customer) : IResponseModel
{
    public ProjectResponseModel() : this(0, "", "") {}
}