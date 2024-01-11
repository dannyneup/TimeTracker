using TimeTracker.UI.Shared.Interfaces;

namespace TimeTracker.UI.Shared.Models.Project;

public record ProjectResponseModel(int Id, string Name, string Customer) : IResponseModel
{
    public ProjectResponseModel() : this(0, "", "") {}
}