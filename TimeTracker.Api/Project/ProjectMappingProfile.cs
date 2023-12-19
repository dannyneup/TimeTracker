using AutoMapper;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectRequestModel>();
        CreateMap<Project, ProjectResponseModel>();
        CreateMap<Project, ProjectWriteViewModel>();

        CreateMap<ProjectWriteViewModel, ProjectWriteViewModel>();
        CreateMap<ProjectWriteViewModel, ProjectRequestModel>();
        
        CreateMap<ProjectReadViewModel, ProjectResponseModel>();
        
        CreateMap<ProjectRequestModel, Project>();

        CreateMap<ProjectResponseModel, ProjectReadViewModel>();
        CreateMap<ProjectResponseModel, ProjectRequestModel>();

    }
}