using Microsoft.EntityFrameworkCore;
using OracleEBSAPI.Data.Entities;

namespace OracleEBSAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}
