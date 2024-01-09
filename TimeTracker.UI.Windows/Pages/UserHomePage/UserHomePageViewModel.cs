using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;
using TimeTracker.UI.Windows.Shared.ViewModels;

namespace TimeTracker.UI.Windows.Pages.UserHomePage;

public sealed class UserHomePageViewModel : ViewModelBase, IPageViewModel
{
    public string Title => Resources.userHomePageTitle;

    public Employee? Employee
    {
        get => _employee; 
        set => SetField(ref _employee, value);
    }
    public ObservableCollection<Project>? AssociatedProjects
    {
        get => _associatedProjects; 
        set => SetField(ref _associatedProjects, value);
    }
    
    private Employee? _employee;
    private ObservableCollection<Project>? _associatedProjects;

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
        AssociatedProjects = new ObservableCollection<Project>(projectList);
    }

    public void OnDeactivated()
    {
        return;
    }
}