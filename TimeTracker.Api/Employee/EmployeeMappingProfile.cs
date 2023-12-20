using AutoMapper;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Employee;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Models.Employee, EmployeeReadViewModel>();
        CreateMap<Models.Employee, EmployeeWriteViewModel>();
        CreateMap<Models.Employee, EmployeeResponseModel>();
        
        CreateMap<EmployeeReadViewModel, Models.Employee>();
        CreateMap<EmployeeReadViewModel, EmployeeWriteViewModel>();
        
        CreateMap<EmployeeWriteViewModel, Models.Employee>();

        CreateMap<EmployeeResponseModel, EmployeeReadViewModel>();
        CreateMap<EmployeeResponseModel, Models.Employee>();
    }
}