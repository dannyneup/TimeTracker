using AutoMapper;
using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Employee;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, EmployeeReadViewModel>();
        CreateMap<Employee, EmployeeWriteViewModel>();
        CreateMap<EmployeeReadViewModel, Employee>();
        CreateMap<EmployeeWriteViewModel, Employee>();
    }
}