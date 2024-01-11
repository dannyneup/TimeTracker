using Avalonia.Controls.ApplicationLifetimes;
using Lamar;
using TimeTracker.UI.Pages.EmployeeOverviewPage;
using TimeTracker.UI.Pages.ProjectOverviewPage;
using TimeTracker.UI.Pages.UserHomePage;
using TimeTracker.UI.Shared;
using TimeTracker.UI.Shell;
using TimeTracker.UI.View;

namespace TimeTracker.UI;

public static class Bootstrapper
{
    public static IContainer Bootstrap(IClassicDesktopStyleApplicationLifetime desktop) =>
        new Container(registry =>
        {
            registry.IncludeRegistry(new ViewRegistry(desktop));
            registry.IncludeRegistry<ShellRegistry>();
            registry.IncludeRegistry<EmployeeOverviewPageRegistry>();
            registry.IncludeRegistry<ProjectOverviewPageRegistry>();
            registry.IncludeRegistry<UserHomePageRegistry>();
            registry.IncludeRegistry<SharedRegistry>();
        });
}