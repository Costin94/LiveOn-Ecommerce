using System;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern - manages transactions and coordinates multiple repositories
    /// Ensures all changes are saved together or none at all (ACID principles)
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // You'll add specific repositories here later, e.g.:
        // IProductRepository Products { get; }
        // IOrderRepository Orders { get; }
        
        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        int Complete();
        
        /// <summary>
        /// Asynchronously saves all changes made in this context to the database
        /// </summary>
        Task<int> CompleteAsync();
    }
}
