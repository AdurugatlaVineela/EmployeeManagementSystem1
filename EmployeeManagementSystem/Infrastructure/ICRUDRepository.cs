using System.Collections.Generic;

namespace EmployeeManagementSystem.Infrastructure
{
    public interface ICRUDRepository<TEntity, TIdentity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetDetails(TIdentity id);
        void Create(TEntity item);
        void update(TEntity item);
        void Delete(TIdentity id); 
    }
} 