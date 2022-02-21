using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Infrastructure
{
    public class CompanyRepository : ICRUDRepository<Company, int>
    {
        EmployeeManagementDBContext _db;

        public CompanyRepository(EmployeeManagementDBContext db)
        {
            _db = db;
        }
 
        public IEnumerable<Company> GetAll()
        {
            return _db.Companies.ToList();
        }

        public Company GetDetails(int id)
        {
             return _db.Companies.FirstOrDefault(c=>c.CompanyId==id);
        }
        public void Create(Company item)
        {
           _db.Companies.Add(item);
           _db.SaveChanges();           
              }

        public void Delete(int id)
        {
            var obj = _db.Companies.FirstOrDefault(c=>c.CompanyId==id);
            if(obj==null)
                return;
            _db.Companies.Remove(obj);
            _db.SaveChanges();
        }

        public void update(Company item)
        {
             var obj = _db.Companies.FirstOrDefault(c=>c.CompanyId==item.CompanyId);
            if(obj==null)
                return;
            obj.CompanyName=item.CompanyName;
            obj.CompanyAddress=item.CompanyAddress;
            obj.City=item.City;
            obj.Country=item.Country;
            obj.PostalCode=item.PostalCode;
            _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();

        }
    }
}