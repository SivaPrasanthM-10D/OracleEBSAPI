using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleEBSAPI.Data;
using OracleEBSAPI.Data.Entities;
using OracleEBSAPI.Models;

namespace OracleEBSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
            => await _context.Employees.Where(e => e.IsActive).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee is null ? NotFound() : employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            //employee.CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            //if (employee.DateOfBirth.HasValue)
            //    employee.DateOfBirth = DateTime.SpecifyKind(employee.DateOfBirth.Value, DateTimeKind.Utc);

            //if (employee.HireDate.HasValue)
            //    employee.HireDate = DateTime.SpecifyKind(employee.HireDate.Value, DateTimeKind.Utc);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updated)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing is null) return NotFound();
            if (id != existing.EmployeeId)
                return BadRequest();
            updated.EmployeeId = id;
            _context.Entry(existing).CurrentValues.SetValues(updated);
            existing.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null) return NotFound();

            employee.IsActive = false;
            employee.UpdatedOn = DateTime.UtcNow;
            employee.UpdatedBy = "soft-delete-api"; // Or capture from authenticated user

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
