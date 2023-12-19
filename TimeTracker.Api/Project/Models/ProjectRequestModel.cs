namespace TimeTracker.Api.Project.Models;

public record ProjectRequestModel(string Name, string Customer, int[] EmployeeIds);
