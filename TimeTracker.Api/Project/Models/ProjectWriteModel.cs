namespace TimeTracker.Api.Project.Models;

public record ProjectWriteModel(string Name, string Customer, int[] EmployeeIds);
