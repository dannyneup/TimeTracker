using AutoMapper;
using TimeTracker.UI.Windows.Pages.ProjectOverviewPage.Records;

namespace TimeTracker.UI.Windows.Shared.Models.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectResponseModel, Project>();
        CreateMap<Project, ProjectRequestModel>();
    }
}