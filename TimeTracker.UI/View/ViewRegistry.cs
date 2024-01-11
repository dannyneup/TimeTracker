using Avalonia.Controls.ApplicationLifetimes;
using Lamar;
using TimeTracker.UI.Shared.Interfaces;

namespace TimeTracker.UI.View;

public class ViewRegistry : ServiceRegistry
{
    public ViewRegistry(IClassicDesktopStyleApplicationLifetime desktop)
    {
        For<IClassicDesktopStyleApplicationLifetime>().Use(desktop).Singleton();
        For<IViewService>().Use<ViewService>().Singleton();
    }
}