namespace TimeTracker.Api.Project.ViewModels;

public record ProjectWriteViewModel(string Name, string Customer, int[] EmployeeIds)
{ 
    public ProjectWriteViewModel() : this("", "", Array.Empty<int>()) {}
}