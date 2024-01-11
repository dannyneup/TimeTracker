using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Shared;
using TimeTracker.UI.Shared.Interfaces;
using TimeTracker.UI.Shared.Interfaces.Repositories;
using TimeTracker.UI.Shared.Models.Employee;

namespace TimeTracker.UI.Pages.EmployeeOverviewPage;

public sealed class EmployeeOverviewPageViewModel : NotifyPropertyChangedBase, IPageViewModel
{
    private readonly IEmployeeRepository _employeeRepository;

    public ObservableCollection<EmployeeResponseModel> Employees
    {
        get => _employees; 
        set => SetField(ref _employees, value);
    }

    public string Title => Resources.employeeOverviewPageTitle;
    
    private ObservableCollection<EmployeeResponseModel> _employees = [];

    public EmployeeOverviewPageViewModel(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task OnActivated()
    {
        var (employees, isSuccess) = await _employeeRepository.GetAllAsync();
        if (!isSuccess) return;
        var employeeList = employees.ToList();
        
        Employees = new ObservableCollection<EmployeeResponseModel>(employeeList);
    }
    
    public void OnDeactivated()
    {
        return;
    }
}