using AutoMapper;
using TimeTracker.UI.Windows.Pages.EmployeeOverviewPage.Records;

namespace TimeTracker.UI.Windows.Shared.Models.Employee;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<EmployeeResponseModel, Employee>();
        CreateMap<Employee, EmployeeRequestModel>();
    }
}