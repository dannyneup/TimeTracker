using TimeTracker.UI.Windows.Shared.Interfaces;

namespace TimeTracker.UI.Windows.Shared.Models.Employee;

public record EmployeeResponseModel(int Id, string LastName, string FirstName, string EmailAddress) : IResponseModel
{
    public EmployeeResponseModel() : this(0,  "", "", "") {}
}