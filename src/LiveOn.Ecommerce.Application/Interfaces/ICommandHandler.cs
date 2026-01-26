using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Interfaces
{
    /// <summary>
    /// Interface for command handlers
    /// Each command should have exactly one handler
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle</typeparam>
    /// <typeparam name="TResult">The type of result returned</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Handles the command execution
        /// </summary>
        TResult Handle(TCommand command);

        /// <summary>
        /// Handles the command execution asynchronously
        /// </summary>
        Task<TResult> HandleAsync(TCommand command);
    }
}
