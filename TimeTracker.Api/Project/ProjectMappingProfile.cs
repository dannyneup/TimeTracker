using AutoMapper;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Models.Project, ProjectWriteModel>();
        CreateMap<Models.Project, ProjectReadModel>();
        CreateMap<Models.Project, ProjectWriteViewModel>();

        CreateMap<ProjectWriteViewModel, ProjectWriteViewModel>();
        CreateMap<ProjectWriteViewModel, ProjectWriteModel>();

        CreateMap<ProjectReadViewModel, ProjectReadModel>();
        CreateMap<ProjectReadViewModel, Models.Project>();
        CreateMap<ProjectReadViewModel, ProjectWriteViewModel>()
            .ForMember(wvm => wvm.EmployeeIds, 
                m => m
                    .MapFrom(rvm => rvm.Employees.Count != 0 ? rvm.Employees.Select(e => e.Id).ToArray() : Array.Empty<int>()));
        
        CreateMap<ProjectWriteModel, Models.Project>();

        CreateMap<ProjectReadModel, ProjectReadViewModel>();
        CreateMap<ProjectReadModel, ProjectWriteModel>();

    }
}