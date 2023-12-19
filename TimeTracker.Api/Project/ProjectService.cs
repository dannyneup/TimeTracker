using AutoMapper;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Project;

public class ProjectService
{
    private readonly IRepository<Project, ProjectRequestModel, ProjectResponseModel> _repository;

    public ProjectService(IRepository<Project, ProjectRequestModel, ProjectResponseModel> repository)
    {
        _repository = repository;
    }

    public Task<ProjectResponseModel> CreateProjectAsync(ProjectRequestModel projectRequest)
    {
        return _repository.AddAsync(projectRequest);
    }

    public Task<List<ProjectResponseModel>> GetAllProjectsAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<ProjectResponseModel> GetProjectByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task EditProject(int id, ProjectRequestModel projectResponse)
    {
        return _repository.UpdateAsync(id, projectResponse);
    }

    public Task DeleteProject(int id)
    {
        return _repository.DeleteAsync(id);
    }
}