using Microsoft.AspNetCore.Mvc;
using OracleEBSAPI.Data.Entities;

namespace OracleEBSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
            => Ok(await _repository.GetActiveEmployeesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee is null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            await _repository.AddAsync(employee);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return NotFound();
            if (id != existing.EmployeeId)
                return BadRequest();
            updated.EmployeeId = id;
            await _repository.UpdateAsync(updated);
            existing.UpdatedOn = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null) return NotFound();

            await _repository.DeleteAsync(employee);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null) return NotFound();

            employee.IsActive = false;
            employee.UpdatedOn = DateTime.UtcNow;
            employee.UpdatedBy = "soft-delete-api";
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
