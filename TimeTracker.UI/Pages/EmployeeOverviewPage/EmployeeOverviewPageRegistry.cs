using Lamar;

namespace TimeTracker.UI.Pages.EmployeeOverviewPage;

public class EmployeeOverviewPageRegistry : ServiceRegistry
{
    public EmployeeOverviewPageRegistry()
    {
        For<EmployeeOverviewPageViewModel>().Use<EmployeeOverviewPageViewModel>();
    }
}