using System.Xml.Serialization;

namespace OracleEBSAPI.Models
{
    [XmlRoot("Employee")]
    public class UpdateEmployeeDto
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? Salary { get; set; }
        public bool IsActive { get; set; }
    }
}
