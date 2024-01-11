using Avalonia.Controls.ApplicationLifetimes;
using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;

namespace TimeTracker.UI.Windows.View;

public sealed class ViewService : IViewService
{
    private readonly IClassicDesktopStyleApplicationLifetime _desktop;

    public ViewService(IClassicDesktopStyleApplicationLifetime desktop)
    {
        _desktop = desktop;
    }

    public void ShowMainWindow(NotifyPropertyChangedBase viewModel)
    {
        _desktop.MainWindow ??= new MainWindow();
        _desktop.MainWindow.DataContext = viewModel;
        _desktop.MainWindow.Show();
    }
    
    
}