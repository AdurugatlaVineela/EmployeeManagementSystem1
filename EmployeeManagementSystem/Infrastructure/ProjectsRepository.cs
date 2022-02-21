using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

using EmployeeManagementSystem.Infrastructure;

namespace EmployeeManagementSystem.Infrastructure
{
    public class ProjectRepository : ICRUDRepository<Project, int>
    {
        EmployeeManagementDBContext _db;

        public ProjectRepository(EmployeeManagementDBContext db)
        {
            _db = db;
        } 

        public IEnumerable<Project> GetAll()
        {
            return _db.Projects.ToList();
        }

        public Project GetDetails(int id)
        {
             return _db.Projects.FirstOrDefault(c=>c.ProjectId==id);
        }
        public void Create(Project item)
        {
           _db.Projects.Add(item);
           _db.SaveChanges();      
             }

        public void Delete(int id)
        {
        var obj = _db.Projects.FirstOrDefault(c=>c.ProjectId==id);
        if(obj==null)
        return;
        _db.Projects.Remove(obj);
        _db.SaveChanges();       
         }

        public void update(Project item)
        {
        var obj = _db.Projects.FirstOrDefault(c=>c.ProjectId==item.ProjectId);
          if(obj==null)
          return;

          obj.ProjectName = item.ProjectName;
          obj.ProjectLocation = item.ProjectLocation;
          obj.ProjectDescription = item.ProjectDescription;
          obj.StartDate = item.StartDate;
          obj.EndDate = item.EndDate;
          _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
          _db.SaveChanges();        }
    }
}