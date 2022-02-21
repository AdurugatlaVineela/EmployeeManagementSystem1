using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Infrastructure
{
     public class DepartmentRepository : ICRUDRepository<Department, int>

    {

        EmployeeManagementDBContext _db;
 
        public DepartmentRepository(EmployeeManagementDBContext db)

        {
 
            _db = db;

        }

        public IEnumerable<Department> GetAll()

        {

            return _db.Departments.ToList();

        }

        public Department GetDetails(int id)

        {

            return _db.Departments.FirstOrDefault(c=>c.DepartmentId==id);

        }

        public void Create(Department item)

        {
            _db.Departments.Add(item);
            _db.SaveChanges();

            //throw new NotImplementedException();

        }

        public void Delete(int id)

        {
            var obj = _db.Departments.FirstOrDefault(c=>c.DepartmentId==id);
            if(obj==null)
            return;
            _db.Departments.Remove(obj);
            _db.SaveChanges();

            //throw new NotImplementedException();

        }

        public void update(Department item)

        {
            var obj = _db.Departments.FirstOrDefault(c=>c.DepartmentId==item.DepartmentId);
            if(obj==null)
                return;
        //    obj.FirstName = item.FirstName;
        //    obj.LastName = item.LastName;
        //    obj.Title =item.Title;
        //     obj.Country = item.Country;
        //     obj.City = item.City;
        //     obj.HireDate = item.HireDate;
        //     obj.BirthDate = item.BirthDate;
        //     obj.Address = item.Address;
               obj.DepartmentName = item.DepartmentName;
               obj.DepartmentLocation= item.DepartmentLocation;
            _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
        
           // throw new NotImplementedException();

        }

    }

}