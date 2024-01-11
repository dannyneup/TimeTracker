using System.Threading.Tasks;

namespace TimeTracker.UI.Shared.Interfaces;

public interface IPageViewModel
{
    string Title { get; }
    Task OnActivated();
    void OnDeactivated();
}