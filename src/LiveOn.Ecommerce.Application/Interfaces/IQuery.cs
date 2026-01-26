namespace LiveOn.Ecommerce.Application.Interfaces
{
    /// <summary>
    /// Marker interface for queries (read operations)
    /// Queries do not modify state and return data
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the query</typeparam>
    public interface IQuery<out TResult>
    {
    }
}
