using System.Collections.Generic;
using TimeTracker.UI.Windows.Shared.Interfaces;

namespace TimeTracker.UI.Windows.Shared.Models.Project;

public class Project : IModel
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Customer { get; set; }
    public List<Employee.Employee> Employees { get; set; }
}