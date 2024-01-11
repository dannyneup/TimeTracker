using Lamar;

namespace TimeTracker.UI.Pages.ProjectOverviewPage;

public class ProjectOverviewPageRegistry : ServiceRegistry
{
    public ProjectOverviewPageRegistry()
    {
        For<ProjectOverviewPageViewModel>().Use<ProjectOverviewPageViewModel>();
    }
}