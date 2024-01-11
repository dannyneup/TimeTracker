using Lamar;

namespace TimeTracker.UI.Windows.Pages.EmployeeOverviewPage;

public class EmployeeOverviewPageRegistry : ServiceRegistry
{
    public EmployeeOverviewPageRegistry()
    {
        For<EmployeeOverviewPageViewModel>().Use<EmployeeOverviewPageViewModel>();
    }
}