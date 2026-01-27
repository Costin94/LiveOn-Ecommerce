using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Domain.Entities;

namespace LiveOn.Ecommerce.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto MapToDto(this User user)
        {
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                IsEmailVerified = user.IsEmailVerified,
                RegistrationDate = user.RegistrationDate,
                LastLoginDate = user.LastLoginDate
            };
        }
    }
}
