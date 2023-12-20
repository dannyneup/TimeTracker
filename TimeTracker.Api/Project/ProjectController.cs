using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Project.ViewModels;
using TimeTracker.Api.Services;

namespace TimeTracker.Api.Project;

[ApiController]
[Route("/projects")]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;
    private readonly IMapper _mapper;
    private readonly ObjectPropertyCheckingService _objectPropertyCheckingService;

    public ProjectController(ProjectService projectService, IMapper mapper, ObjectPropertyCheckingService objectPropertyCheckingService)
    {
        _projectService = projectService;
        _mapper = mapper;
        _objectPropertyCheckingService = objectPropertyCheckingService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProjectWriteViewModel projectWriteViewModel)
    {
        if (_objectPropertyCheckingService.HasNullOrEmptyProperties(projectWriteViewModel)) return BadRequest();
        var projectRequest = _mapper.Map<ProjectRequestModel>(projectWriteViewModel);
        var projectResponse = await _projectService.CreateProjectAsync(projectRequest);

        var projectReadViewModel = _mapper.Map<ProjectReadViewModel>(projectResponse);
    
        return CreatedAtAction("GetById", new {id = projectReadViewModel.Id}, projectReadViewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();

        var projectReadViewModels = _mapper.Map<List<ProjectReadViewModel>>(projects);
        
        return Ok(projectReadViewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        var projectReadViewModel = _mapper.Map<ProjectReadViewModel>(project);
        
        return projectReadViewModel != null
            ? Ok(projectReadViewModel)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, ProjectWriteViewModel projectWriteViewModel)
    {
        if (_objectPropertyCheckingService.HasNullOrEmptyProperties(projectWriteViewModel)) return BadRequest();
        if (await _projectService.GetProjectByIdAsync(id) == null) return NotFound();
        var projectRequest = _mapper.Map<ProjectRequestModel>(projectWriteViewModel);
        await _projectService.EditProject(id, projectRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _projectService.DeleteProject(id);

        return NoContent();
    }
}