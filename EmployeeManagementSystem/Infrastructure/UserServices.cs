using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Infrastructure
{
    public interface IUserService 
    {
        bool Authenticate(User item); 
    //ADDED: a function declaration to get the user role from the DB. 
        Role GetUserRole(int id);
        List<User> GetAll(); 
        User GetDetails(int id);
    } 
    public class UserService : IUserService
    {
        EmployeeManagementDBContext _context; 
        public UserService(EmployeeManagementDBContext context) => _context = context;
        public bool Authenticate(User item)
        {
            var obj = _context.Users.FirstOrDefault(
                c=>c.UserName.Equals(item.UserName) && c.Password.Equals(item.Password) );
            if(obj != null){
                item.Id = obj.Id;
                return true;
            } 
            else 
                return false;
        }
//ADDED: the implementation to get the Role for the currently logged in user. 
        public Role GetUserRole(int id)
        {
            var roles = _context.Roles.FromSqlRaw(
                $"SELECT RoleId, RoleName FROM Roles WHERE RoleId IN " + 
                $" (SELECT RoleID FROM UserRoles WHERE UserId={id})"
            );
            if(roles.Count()==0)
            return null; 
            else 
            return roles.First();
        }
//END: of editing the function.
        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }
        public User GetDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}