using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Mappers;
using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Services
{
    /// <summary>
    /// Service for user management - handles all user-related business logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public async Task<UserDto> CreateUserAsync(string email, string firstName, string lastName, string passwordHash, UserRole role, string phoneNumber = null)
        {
            // Create new user entity
            var user = new User(
                email: email,
                firstName: firstName,
                lastName: lastName,
                passwordHash: passwordHash,
                role: role
            );

            // Set optional phone number
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                user.SetPhoneNumber(phoneNumber);
            }

            // Save to database
            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            // Return DTO
            return user.MapToDto();
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        public async Task<bool> UpdateUserAsync(int id, string email = null, string firstName = null, string lastName = null, string phoneNumber = null)
        {
            // Get user
            var user = await _unitOfWork.User.GetByIdAsync(id);
            
            if (user == null)
                return false;

            // Update only provided fields
            if (!string.IsNullOrEmpty(email))
                user.SetEmail(email);

            if (!string.IsNullOrEmpty(firstName))
                user.SetFirstName(firstName);

            if (!string.IsNullOrEmpty(lastName))
                user.SetLastName(lastName);

            if (!string.IsNullOrEmpty(phoneNumber))
                user.SetPhoneNumber(phoneNumber);

            // Save changes
            _unitOfWork.User.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Soft deletes a user (sets IsDeleted = true)
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            
            if (user == null)
                return false;

            // Soft delete
            _unitOfWork.User.Remove(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Activates a user account
        /// </summary>
        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            
            if (user == null)
                return false;

            user.Activate();
            _unitOfWork.User.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Deactivates a user account
        /// </summary>
        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            
            if (user == null)
                return false;

            user.Deactivate();
            _unitOfWork.User.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            return user?.MapToDto();
        }

        /// <summary>
        /// Gets a user by email
        /// </summary>
        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.User.GetByEmailAsync(email);
            return user?.MapToDto();
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            return users.Select(u => u.MapToDto());
        }

        /// <summary>
        /// Gets all active users
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
        {
            var users = await _unitOfWork.User.GetActiveUsersAsync();
            return users.Select(u => u.MapToDto());
        }

        /// <summary>
        /// Gets all users with a specific role
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(UserRole role)
        {
            var users = await _unitOfWork.User.GetByRoleASync(role);
            return users.Select(u => u.MapToDto());
        }
    }
}
