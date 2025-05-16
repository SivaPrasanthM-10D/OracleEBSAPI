using OracleEBSAPI.Data.Entities;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task SaveChangesAsync();
}