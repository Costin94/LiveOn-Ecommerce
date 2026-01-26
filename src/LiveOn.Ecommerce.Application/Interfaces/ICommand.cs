namespace LiveOn.Ecommerce.Application.Interfaces
{
    /// <summary>
    /// Marker interface for commands (write operations)
    /// Commands modify state and return a result
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    public interface ICommand<out TResult>
    {
    }
}
