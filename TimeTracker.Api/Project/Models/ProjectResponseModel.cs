using TimeTracker.Api.Employee;
using TimeTracker.Api.Employee.Models;

namespace TimeTracker.Api.Project.Models;

public record ProjectResponseModel(int Id, string Name, string Customer, List<EmployeeResponseModel> Employees);