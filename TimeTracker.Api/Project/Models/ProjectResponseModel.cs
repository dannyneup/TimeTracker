using TimeTracker.Api.Employee;

namespace TimeTracker.Api.Project.Models;

public record ProjectResponseModel(int Id, string Name, string Customer, List<EmployeeResponseModel> Employees);