using System.Collections.Generic;
using System.Dynamic;
using IModel = TimeTracker.UI.Windows.Shared.Interfaces.IModel;

namespace TimeTracker.UI.Windows.Shared.Models.Employee;

public class Employee : IModel
{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public IEnumerable<Project.Project> AssociatedProjects { get; set; }
}