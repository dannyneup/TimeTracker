using AutoMapper;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectReadViewModel>();
        CreateMap<Project, ProjectWriteViewModel>();
        CreateMap<ProjectWriteViewModel, Project>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeIds.Select(id => new Employee.Employee { Id = id })));
        CreateMap<ProjectReadViewModel, Project>();
        CreateMap<ProjectReadViewModel, ProjectWriteViewModel>();
        
    }
}