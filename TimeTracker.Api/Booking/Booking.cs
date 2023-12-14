using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Api.Booking;

[Table("Booking")]
public class Booking
{
    [Key]
    public required int Id { get; init; }
    [Required]
    public required Employee.Employee Employee { get; set; }
    [Required]
    public required Project.Project Project { get; set; }
    
    [Required]
    public required DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}