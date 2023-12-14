using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Project;

[ApiController]
[Route("/projects")]
public class ProjectController : ControllerBase
{
    private readonly TimeTrackerContext _context;
    private readonly IMapper _mapper;

    public ProjectController(TimeTrackerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProjectWriteViewModel projectWriteViewModel)
    {
        var project = _mapper.Map<Project>(projectWriteViewModel);
        var projectEntity = _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var projectReadViewModel = _mapper.Map<ProjectReadViewModel>(projectEntity.Entity);
    
        return CreatedAtAction("GetById", new {id = projectReadViewModel.Id}, projectReadViewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var projects = await _context.Projects
            .ToListAsync();

        var projectReadViewModels = _mapper.Map<List<ProjectReadViewModel>>(projects);
        
        return Ok(projectReadViewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        var projectReadViewModel = _mapper.Map<ProjectReadViewModel>(project);
        
        return projectReadViewModel != null
            ? Ok(projectReadViewModel)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, ProjectWriteViewModel projectWriteViewModel)
    {
        if (!await EntityExists(id)) return NotFound();

        var project = new Project()
        {
            Id = id,
            Name = projectWriteViewModel.Name,
            Customer = projectWriteViewModel.Customer
        };

        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project is null) return NotFound();
        _context.Projects.Remove(project);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private Task<bool> EntityExists(int id)
    {
        return _context.Projects.AnyAsync(p => p.Id == id);
    }
}