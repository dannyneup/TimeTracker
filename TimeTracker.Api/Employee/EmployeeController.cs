using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Employee.ViewModels;
using TimeTracker.Api.Services;

namespace TimeTracker.Api.Employee;

[ApiController]
[Route("/employees")]
public class EmployeeController : ControllerBase
{
    private readonly EmailValidationService _emailValidationService;
    private readonly EmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ObjectPropertyCheckingService _objectPropertyCheckingService;

    public EmployeeController(EmployeeService employeeService, IMapper mapper,
        EmailValidationService emailValidationService, ObjectPropertyCheckingService objectPropertyCheckingService)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _emailValidationService = emailValidationService;
        _objectPropertyCheckingService = objectPropertyCheckingService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(EmployeeWriteViewModel employeeWriteViewModel)
    {
        if (_objectPropertyCheckingService.HasNullOrEmptyProperties(employeeWriteViewModel)) return BadRequest();
        if (!_emailValidationService.IsValidEmail(employeeWriteViewModel.EmailAddress))
            return BadRequest("emailAddress is invalid");
        var employeeWrite = _mapper.Map<EmployeeWriteModel>(employeeWriteViewModel);
        var employeeRead = await _employeeService.CreateEmployeeAsync(employeeWrite);

        var employeeReadViewModel = _mapper.Map<EmployeeReadViewModel>(employeeRead);

        return CreatedAtAction("GetById", new { id = employeeReadViewModel.Id }, employeeReadViewModel);
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();

        var employeeReadViewModels = _mapper.Map<List<EmployeeReadViewModel>>(employees);

        return Ok(employeeReadViewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        var employeeReadViewModel = _mapper.Map<EmployeeReadViewModel>(employee);

        return employeeReadViewModel != null
            ? Ok(employeeReadViewModel)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, EmployeeWriteViewModel employeeWriteViewModel)
    {
        if (_objectPropertyCheckingService.HasNullOrEmptyProperties(employeeWriteViewModel)) return BadRequest();
        if (await _employeeService.GetEmployeeByIdAsync(id) == null) return NotFound();
        var employeeWrite = _mapper.Map<EmployeeWriteModel>(employeeWriteViewModel);
        await _employeeService.EditEmployee(id, employeeWrite);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployee(id);

        return NoContent();
    }

    [HttpGet("{id}/projects")]
    public async Task<ActionResult> GetEmployeesProjects(int id)
    {
        if (await _employeeService.GetEmployeeByIdAsync(id) == null) return NotFound();

        var projects = await _employeeService.GetEmployeesProjectsAsync(id);

        return Ok(projects);
    }

    [HttpGet("{id}/workingHours")]
    public async Task<ActionResult> GetEmployeesWorkingHours(int id, [FromQuery] DateTimeOffset? filterStart,
        DateTimeOffset? filterEnd)
    {
        var badRequest = CheckTimestampFilterDateTimes(filterStart, filterEnd);
        if (badRequest != null) return badRequest;

        var workingHours =
            await _employeeService.GetEmployeesWorkingHoursAsync(id, (DateTimeOffset)filterStart!,
                (DateTimeOffset)filterEnd!);
        return Ok(workingHours);
    }

    [HttpGet("{id}/workingHoursDeviation")]
    public async Task<ActionResult> GetEmployeesWorkingHoursDeviation(int id, [FromQuery] DateTimeOffset? filterStart,
        DateTimeOffset? filterEnd)
    {
        var badRequest = CheckTimestampFilterDateTimes(filterStart, filterEnd);
        if (badRequest != null) return badRequest;

        var workingHoursDeviation =
            await _employeeService.GetEmployeesWorkingHoursDeviationAsync(id, (DateTimeOffset)filterStart!,
                (DateTimeOffset)filterEnd!);
        return Ok(workingHoursDeviation);
    }

    private BadRequestObjectResult? CheckTimestampFilterDateTimes(DateTimeOffset? start, DateTimeOffset? end)
    {
        if (end == null || start == null)
            return BadRequest("filterStart and filterEnd are required");
        if (end < start) return BadRequest("timestampStart must be before timestampEnd");
        return null;
    }
}