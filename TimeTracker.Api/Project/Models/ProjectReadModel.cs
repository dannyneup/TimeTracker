using TimeTracker.Api.Employee.Models;

namespace TimeTracker.Api.Project.Models;

public record ProjectReadModel(int Id, string Name, string Customer, List<EmployeeResponseModel> Employees);