using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Interfaces
{
    /// <summary>
    /// Interface for query handlers
    /// Each query should have exactly one handler
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle</typeparam>
    /// <typeparam name="TResult">The type of result returned</typeparam>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handles the query execution
        /// </summary>
        TResult Handle(TQuery query);

        /// <summary>
        /// Handles the query execution asynchronously
        /// </summary>
        Task<TResult> HandleAsync(TQuery query);
    }
}
