using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Domain.Interfaces
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// You'll implement this in the Infrastructure layer
    /// </summary>
    public interface IRepository<T> where T : class
    {
        // Query methods
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        
        // Command methods
        void Add(T entity);
        Task AddAsync(T entity);
        
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        
        void Update(T entity);
        
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
