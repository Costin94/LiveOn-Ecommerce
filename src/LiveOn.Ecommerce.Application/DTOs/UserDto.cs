using System;
using LiveOn.Ecommerce.Domain.Enums;

namespace LiveOn.Ecommerce.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for User
    /// Used to transfer user data between layers (without sensitive info)
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
