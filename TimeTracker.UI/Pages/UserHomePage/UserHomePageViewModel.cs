using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Shared;
using TimeTracker.UI.Shared.Interfaces;
using TimeTracker.UI.Shared.Interfaces.Repositories;
using TimeTracker.UI.Shared.Models;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;

namespace TimeTracker.UI.Pages.UserHomePage;

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

    public TimeSpan? TodaysTotalWorkingHours
    {
        get => _todaysTotalWorkingHours;
        set => SetField(ref _todaysTotalWorkingHours, value);
    }
    private TimeSpan? _todaysTotalWorkingHours;

    public ObservableDictionary<int, TimeSpan> TodaysWorkingHoursPerProject
    {
        get => _todaysWorkingHoursPerProject;
        set => SetField(ref _todaysWorkingHoursPerProject, value);
    }
    private ObservableDictionary<int, TimeSpan> _todaysWorkingHoursPerProject;
    
    public int ActiveProjectId => 1;
    
    private readonly IEmployeeRepository _employeeRepository;
    
    public UserHomePageViewModel(
        IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task OnActivated()
    {
        await LoadAssociatedProjects();
    }

    public void OnDeactivated()
    {
        return;
    }

    private async Task LoadAssociatedProjects()
    {
        var (projects, isSuccess) = await _employeeRepository.GetAssociatedProjectsAsync(1);
        if (!isSuccess) return;
        var projectList = projects.ToList();
        AssociatedProjects = new ObservableCollection<ProjectResponseModel>(projectList);
    }

    private async Task LoadTodaysTotalWorkingHours()
    {
        throw new NotImplementedException();
    }
    
    private async Task LoadTodaysWorkingHoursPerProject()
    {
        throw new NotImplementedException();
    }
}