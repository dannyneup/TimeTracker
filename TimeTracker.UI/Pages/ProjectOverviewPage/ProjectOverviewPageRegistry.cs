using Lamar;

namespace TimeTracker.UI.Windows.Pages.ProjectOverviewPage;

public class ProjectOverviewPageRegistry : ServiceRegistry
{
    public ProjectOverviewPageRegistry()
    {
        For<ProjectOverviewPageViewModel>().Use<ProjectOverviewPageViewModel>();
    }
}