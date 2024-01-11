using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;

namespace TimeTracker.UI.Windows.Pages.UserHomePage;

public sealed class UserHomePageViewModel : NotifyPropertyChangedBase, IPageViewModel
{
    public string Title => Resources.userHomePageTitle;

    public EmployeeResponseModel? Employee
    {
        get => _employee; 
        set => SetField(ref _employee, value);
    }
    private EmployeeResponseModel? _employee;
    
    public ObservableCollection<ProjectResponseModel>? AssociatedProjects
    {
        get => _associatedProjects; 
        set => SetField(ref _associatedProjects, value);
    }
    private ObservableCollection<ProjectResponseModel>? _associatedProjects;

    public int ActiveProjectId => 1;
    
    private readonly IEmployeeRepository _employeeRepository;
    
    public UserHomePageViewModel(
        IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task OnActivated()
    {
        var (projects, isSuccess) = await _employeeRepository.GetAssociatedProjectsAsync(1);
        if (!isSuccess) return;
        var projectList = projects.ToList();
        AssociatedProjects = new ObservableCollection<ProjectResponseModel>(projectList);
    }

    public void OnDeactivated()
    {
        return;
    }
}