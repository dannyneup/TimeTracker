using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.ViewModels;

namespace TimeTracker.UI.Windows.Pages.EmployeeOverviewPage;

public sealed class EmployeeOverviewPageViewModel : ViewModelBase, IPageViewModel
{
    private readonly IEmployeeRepository _employeeRepository;

    public ObservableCollection<Employee> Employees
    {
        get => _employees; 
        set => SetField(ref _employees, value);
    }

    public string Title => Resources.employeeOverviewPageTitle;
    
    private ObservableCollection<Employee> _employees = [];

    public EmployeeOverviewPageViewModel(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task OnActivated()
    {
        var (employees, isSuccess) = await _employeeRepository.GetAllAsync();
        Console.WriteLine(employees.FirstOrDefault().ToString());
        Console.Write(isSuccess);
        if (!isSuccess) return;
        var employeeList = employees.ToList();
        
        Employees = new ObservableCollection<Employee>(employeeList);
    }
    
    public void OnDeactivated()
    {
        return;
    }
}