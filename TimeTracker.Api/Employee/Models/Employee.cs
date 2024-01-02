using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Api.Employee.Models;
[Table("Employee")]
public class Employee : IEquatable<Employee>
{
    [Key]
    public int Id { get; set; }
    
    public string LastName { get; set; } = null!;
    
    public string FirstName { get; set; } = null!;
    
    public string EmailAddress { get; set; } = null!;

    public List<Project.Models.Project> Projects { get; set; } = [];

    public List<Booking.Models.Booking> Bookings { get; set; } = [];

    public bool Equals(Employee? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && LastName == other.LastName && FirstName == other.FirstName && EmailAddress == other.EmailAddress;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Employee)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, LastName, FirstName, EmailAddress);
    }
}