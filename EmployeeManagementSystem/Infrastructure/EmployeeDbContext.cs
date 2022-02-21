using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Infrastructure
{
    public class EmployeeManagementDBContext : DbContext
    {
        public EmployeeManagementDBContext(DbContextOptions<EmployeeManagementDBContext> options) : base(options) { }

       public DbSet<Employee> Employees {get; set;}
           
        public DbSet<Project> Projects {get; set;}
 
        public DbSet<Company> Companies {get; set;}

        public DbSet<Department> Departments {get; set;}

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
   

    }
}