using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Project.ViewModels;

public record ProjectReadViewModel(int Id, string Name, string Customer, List<EmployeeReadViewModel> Employees);