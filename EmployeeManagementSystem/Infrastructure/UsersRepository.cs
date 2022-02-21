using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Infrastructure
{
    public class UserRepository : ICRUDRepository<User, int>
    {
        EmployeeManagementDBContext _db;

        public UserRepository(EmployeeManagementDBContext db)
        {
            _db =db;
        } 

        public IEnumerable<User> GetAll()
        {
            return   _db.Users.ToList();
        }

        public User GetDetails(int id)
        {
             return _db.Users.FirstOrDefault(c=>c.Id==id);
        }

        public void Create(User item)
        {
          _db.Users.Add(item);
           _db.SaveChanges();         
               }

        public void Delete(int id) 
        {
            var obj = _db.Users.FirstOrDefault(c=>c.Id==id);
            if(obj==null)
                return;
            _db.Users.Remove(obj);
            _db.SaveChanges();
        }

        public void update(User item) 
        {
             var obj = _db.Users.FirstOrDefault(c=>c.Id==item.Id);
            if(obj==null)
                return;
            obj.UserName=item.UserName;
            obj.Password=item.Password;
            _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();

        }
    }
}