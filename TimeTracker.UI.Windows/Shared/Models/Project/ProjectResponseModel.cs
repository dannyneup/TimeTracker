using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Models;

namespace TimeTracker.UI.Windows.Pages.ProjectOverviewPage.Records;

public record ProjectResponseModel(int Id, string Name, string Customer) : IResponseModel
{
    public ProjectResponseModel() : this(0, "", "") {}
}