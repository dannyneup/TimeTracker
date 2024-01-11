using System.Threading.Tasks;
using TimeTracker.UI.Pages.EmployeeOverviewPage;
using TimeTracker.UI.Pages.ProjectOverviewPage;
using TimeTracker.UI.Pages.UserHomePage;
using TimeTracker.UI.Shared;
using TimeTracker.UI.Shared.Interfaces;

namespace TimeTracker.UI.Shell;

public sealed class ApplicationViewModel : NotifyPropertyChangedBase
{
    public IPageViewModel[] Pages { get; }
    public IPageViewModel? ActivePage
    {
        get => _activePage;
        set
        {
            if (SetField(ref _activePage, value) && _activePage != null)
            {
                Task.Run(async () => await NavigateTo(_activePage));
            }
        }
    }

    private readonly IViewService _viewService;
    private readonly EmployeeOverviewPageViewModel _employeeOverviewPageViewModel;
    private readonly ProjectOverviewPageViewModel _projectOverviewPageViewModel;
    private readonly UserHomePageViewModel _userHomePageViewModel;

    private IPageViewModel? _activePage;

    public ApplicationViewModel(
        IViewService viewService, 
        EmployeeOverviewPageViewModel employeeOverviewPageViewModel,
        ProjectOverviewPageViewModel projectOverviewPageViewModel,
        UserHomePageViewModel userHomePageViewModel
        )
    {
        _viewService = viewService;
        _employeeOverviewPageViewModel = employeeOverviewPageViewModel;
        _projectOverviewPageViewModel = projectOverviewPageViewModel;
        _userHomePageViewModel = userHomePageViewModel;

        Pages = [_userHomePageViewModel, _employeeOverviewPageViewModel, _projectOverviewPageViewModel];
    }

    public void Start()
    {
        _viewService.ShowMainWindow(this);
        Task.Run(async () => await NavigateTo(_userHomePageViewModel));
    }
    
    private async Task NavigateTo(IPageViewModel pageViewModel)
    {
        ActivePage?.OnDeactivated();
        ActivePage = pageViewModel;
        await ActivePage.OnActivated();
    }
}