using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Infrastructure
{
    public class EmployeeRepository : ICRUDRepository<Employee, int>
    {
        EmployeeManagementDBContext _db;

        public EmployeeRepository(EmployeeManagementDBContext db)
        {
            _db = db;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees.ToList();
        }

        public Employee GetDetails(int id)
        {
             return _db.Employees.FirstOrDefault(c=>c.EmployeeId==id);
        }
        public void Create(Employee item)
        {
            _db.Employees.Add(item);
            _db.SaveChanges();
        } 

        public void Delete(int id)
        {
            var obj = _db.Employees.FirstOrDefault(c=>c.EmployeeId==id);
            if(obj==null)
                return;
            _db.Employees.Remove(obj);
            _db.SaveChanges();
        }

        public void update(Employee item)
        {
             var obj = _db.Employees.FirstOrDefault(c=>c.EmployeeId==item.EmployeeId);
            if(obj==null)
                return;
            obj.LastName=item.LastName;
            obj.FirstName= item.FirstName;
            obj.BirthDate=item.BirthDate;
            obj.HireDate=item.HireDate;
            obj.Address=item.Address;
            obj.City=item.City;
            obj.Region=item.Region;
            obj.PostalCode=item.PostalCode;
            obj.Country=item.Country;
            obj.HomePhone=item.HomePhone;
            obj.DepartmentId=item.DepartmentId;
            obj.ProjectId=item.ProjectId;
            _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();

        }
    }
}