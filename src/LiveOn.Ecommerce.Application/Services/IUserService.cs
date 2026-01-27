using System.Collections.Generic;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Domain.Enums;

namespace LiveOn.Ecommerce.Application.Services
{
    /// <summary>
    /// Service interface for user management operations
    /// </summary>
    public interface IUserService
    {
        // Create
        Task<UserDto> CreateUserAsync(string email, string firstName, string lastName, string passwordHash, UserRole role, string phoneNumber = null);

        // Update
        Task<bool> UpdateUserAsync(int id, string email = null, string firstName = null, string lastName = null, string phoneNumber = null);

        // Delete (soft delete)
        Task<bool> DeleteUserAsync(int id);

        // Activate/Deactivate
        Task<bool> ActivateUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);

        // Queries
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role);
    }
}
