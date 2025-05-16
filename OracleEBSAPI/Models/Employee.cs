using System.ComponentModel.DataAnnotations;

namespace OracleEBSAPI.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public bool IsActive { get; set; } = true;

    // Audit fields
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
