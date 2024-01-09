using System.Threading.Tasks;

namespace TimeTracker.UI.Windows.Shared.Interfaces;

public interface IPageViewModel
{
    string Title { get; }
    Task OnActivated();
    void OnDeactivated();
}