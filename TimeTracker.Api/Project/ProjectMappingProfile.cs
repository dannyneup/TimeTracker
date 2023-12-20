using AutoMapper;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Models.Project, ProjectRequestModel>();
        CreateMap<Models.Project, ProjectResponseModel>();
        CreateMap<Models.Project, ProjectWriteViewModel>();

        CreateMap<ProjectWriteViewModel, ProjectWriteViewModel>();
        CreateMap<ProjectWriteViewModel, ProjectRequestModel>();

        CreateMap<ProjectReadViewModel, ProjectResponseModel>();
        CreateMap<ProjectReadViewModel, Models.Project>();
        CreateMap<ProjectReadViewModel, ProjectWriteViewModel>()
            .ForMember(wvm => wvm.EmployeeIds, 
                m => m
                    .MapFrom(rvm => rvm.Employees.Count != 0 ? rvm.Employees.Select(e => e.Id).ToArray() : Array.Empty<int>()));
        
        CreateMap<ProjectRequestModel, Models.Project>();

        CreateMap<ProjectResponseModel, ProjectReadViewModel>();
        CreateMap<ProjectResponseModel, ProjectRequestModel>();

    }
}