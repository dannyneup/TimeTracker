using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee.ViewModels;
using TimeTracker.Api.Services;

namespace TimeTracker.Api.Employee;

[ApiController]
[Route("/employees")]
public class EmployeeController : ControllerBase
{
    private readonly TimeTrackerContext _context;
    private readonly IMapper _mapper;
    private readonly EmailValidationService _emailValidationService;

    public EmployeeController(TimeTrackerContext context, IMapper mapper, EmailValidationService emailValidationService)
    {
        _context = context;
        _mapper = mapper;
        _emailValidationService = emailValidationService;
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(EmployeeWriteViewModel employeeWriteViewModel)
    {
        var employee = _mapper.Map<Employee>(employeeWriteViewModel);
        
        var employeeEntry = await _context.AddAsync(employee);
        await _context.SaveChangesAsync();

        var employeeReadViewModel = _mapper.Map<EmployeeReadViewModel>(employeeEntry.Entity);
    
        return CreatedAtAction("GetById", new { id = employeeReadViewModel.Id}, employeeReadViewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var employees = await _context.Employees
            .ToListAsync();

        var employeeReadViewModels = _mapper.Map<List<EmployeeReadViewModel>>(employees);
    
        return Ok(employeeReadViewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        var employeeReadViewModel = _mapper.Map<EmployeeReadViewModel>(employee);
        
        return employeeReadViewModel != null
            ? Ok(employeeReadViewModel)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, EmployeeWriteViewModel inputEmployeeWriteViewModel)
    {
        if (!await EntityExists(id)) return NotFound();
        if (!_emailValidationService.IsValidEmail(inputEmployeeWriteViewModel.EmailAddress))
            return BadRequest("invalid Email-Address");

        var employee = _mapper.Map<Employee>(inputEmployeeWriteViewModel);
        employee.Id = id;
        
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee is null) return NotFound();
        _context.Employees.Remove(employee);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/projects")]
    public async Task<ActionResult> GetEmployeesProjects(int id)
    {
        if (!await EntityExists(id)) return NotFound();

        var employee = await _context.Employees.Include(e => e.Projects).SingleOrDefaultAsync(x => x.Id == id);
        var projects = employee!.Projects;
        
        return Ok(projects);
    }
    
    private Task<bool> EntityExists(int id)
    {
        return _context.Employees.AnyAsync(e => e.Id == id);
    }
}