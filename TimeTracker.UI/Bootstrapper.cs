using Avalonia.Controls.ApplicationLifetimes;
using Lamar;
using TimeTracker.UI.Windows.Pages.EmployeeOverviewPage;
using TimeTracker.UI.Windows.Pages.ProjectOverviewPage;
using TimeTracker.UI.Windows.Pages.UserHomePage;
using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shell;
using TimeTracker.UI.Windows.View;

namespace TimeTracker.UI.Windows;

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