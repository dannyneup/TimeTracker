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
        CreateMap<Models.Employee, EmployeeReadModel>();

        CreateMap<EmployeeWriteViewModel, EmployeeWriteModel>();

        CreateMap<EmployeeReadViewModel, Models.Employee>();
        CreateMap<EmployeeReadViewModel, EmployeeReadModel>();
        CreateMap<EmployeeReadViewModel, EmployeeWriteViewModel>();
        
        CreateMap<EmployeeWriteModel, Models.Employee>();

        CreateMap<EmployeeReadModel, EmployeeReadViewModel>();
        CreateMap<EmployeeReadModel, Models.Employee>();
    }
}