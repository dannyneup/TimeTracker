using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TimeTracker.UI.Windows.Shell;

namespace TimeTracker.UI.Windows;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var container = Bootstrapper.Bootstrap(desktop);
            desktop.Exit += (_, _) => container.Dispose();

            container.GetInstance<ApplicationViewModel>().Start();
        }

        base.OnFrameworkInitializationCompleted();
    }
}