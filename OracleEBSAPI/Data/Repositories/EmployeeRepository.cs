using Microsoft.EntityFrameworkCore;
using OracleEBSAPI.Data;
using OracleEBSAPI.Data.Entities;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    public EmployeeRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        => await _context.Employees.Where(e => e.IsActive).ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await _context.Employees.FindAsync(id);

    public async Task AddAsync(Employee employee)
        => await _context.Employees.AddAsync(employee);

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}