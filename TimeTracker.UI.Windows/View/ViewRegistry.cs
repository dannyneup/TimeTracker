using Avalonia.Controls.ApplicationLifetimes;
using Lamar;
using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;

namespace TimeTracker.UI.Windows.View;

public class ViewRegistry : ServiceRegistry
{
    public ViewRegistry(IClassicDesktopStyleApplicationLifetime desktop)
    {
        For<IClassicDesktopStyleApplicationLifetime>().Use(desktop).Singleton();
        For<IViewService>().Use<ViewService>().Singleton();
    }
}