using System.Drawing;
using AutoMapper;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectReadViewModel>();
        CreateMap<Project, ProjectWriteViewModel>();
        CreateMap<ProjectWriteViewModel, Project>();
        CreateMap<ProjectReadViewModel, Project>();
        CreateMap<ProjectReadViewModel, ProjectWriteViewModel>();
    }
}