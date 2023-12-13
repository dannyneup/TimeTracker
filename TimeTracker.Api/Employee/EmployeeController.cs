using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Employee;

[ApiController]
[Route("/employees")]
public class EmployeeController(TimeTrackerContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(Employee employee)
    {
        context.Add(employee);
    
        await context.SaveChangesAsync();
    
        return CreatedAtAction("GetById", new { id = employee.Id}, employee);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var employees = await context.Employees
            .ToListAsync();
    
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var employee = await context.Employees.FindAsync(id);

        return employee != null
            ? Ok(employee)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, Employee inputEmployee)
    {
        if (id != inputEmployee.Id) return BadRequest();

        if (!await EntityExists(id)) return NotFound();
        
        context.Entry(inputEmployee).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await context.Employees.FindAsync(id);

        if (employee is null) return NotFound();
        context.Employees.Remove(employee);

        await context.SaveChangesAsync();

        return NoContent();
    }
    
    private Task<bool> EntityExists(int id)
    {
        return context.Employees.AnyAsync(e => e.Id == id);
    }
}