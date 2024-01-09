using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Models;

namespace TimeTracker.UI.Windows.Pages.EmployeeOverviewPage.Records;

public record EmployeeResponseModel(int Id, string LastName, string FirstName, string EmailAddress) : IResponseModel
{
    public EmployeeResponseModel() : this(0,  "", "", "") {}
}