using TimeTracker.UI.Shared.Interfaces;

namespace TimeTracker.UI.Shared.Models.Employee;

public record EmployeeResponseModel(int Id, string LastName, string FirstName, string EmailAddress) : IResponseModel
{
    public EmployeeResponseModel() : this(0,  "", "", "") {}
}