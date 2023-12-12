using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Project;

[ApiController]
[Route("/projects")]
public class ProjectController(TimeTrackerContext context) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> Create(Project project)
    {
        context.Add(project);
    
        await context.SaveChangesAsync();
    
        return CreatedAtAction("GetById", new {id = project.Id}, project);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var projects = await context.Projects
            .ToListAsync();
    
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var project = await context.Projects.FindAsync(id);

        return project != null
            ? Ok(project)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project is null) return NotFound();

        project.Name = project.Name;
        project.Customer = project.Customer;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project is null) return NotFound();
        context.Remove(project);

        await context.SaveChangesAsync();

        return NoContent();
    }
}