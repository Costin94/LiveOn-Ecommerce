using System;
using LiveOn.Ecommerce.Domain.Common;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Exceptions;

namespace LiveOn.Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a user/customer in the e-commerce system
    /// This is an Aggregate Root
    /// </summary>
    public class User : BaseEntity
    {
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _passwordHash;

        // Private constructor for EF
        private User()
        {
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public User(string email, string firstName, string lastName, string passwordHash, UserRole role = UserRole.Customer)
        {
            SetEmail(email);
            SetFirstName(firstName);
            SetLastName(lastName);
            _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
            IsActive = true;
            IsEmailVerified = false;
            RegistrationDate = DateTime.UtcNow;
            LastLoginDate = null;
        }

        /// <summary>
        /// User's email address (unique, required, max 256 characters)
        /// </summary>
        public string Email
        {
            get => _email;
            private set => _email = value;
        }

        /// <summary>
        /// User's first name (required, max 50 characters)
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            private set => _firstName = value;
        }

        /// <summary>
        /// User's last name (required, max 50 characters)
        /// </summary>
        public string LastName
        {
            get => _lastName;
            private set => _lastName = value;
        }

        /// <summary>
        /// Full name (computed property)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Phone number (optional, max 20 characters)
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            private set => _phoneNumber = value;
        }

        /// <summary>
        /// Hashed password (never store plain text!)
        /// </summary>
        public string PasswordHash
        {
            get => _passwordHash;
            private set => _passwordHash = value;
        }

        /// <summary>
        /// User role (Customer, Manager, Administrator)
        /// </summary>
        public UserRole Role { get; private set; }

        /// <summary>
        /// Whether the user account is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Whether the email has been verified
        /// </summary>
        public bool IsEmailVerified { get; private set; }

        /// <summary>
        /// Date and time when the user registered
        /// </summary>
        public DateTime RegistrationDate { get; private set; }

        /// <summary>
        /// Date and time of last login
        /// </summary>
        public DateTime? LastLoginDate { get; private set; }

        /// <summary>
        /// Number of failed login attempts (for account lockout)
        /// </summary>
        public int FailedLoginAttempts { get; private set; }

        /// <summary>
        /// Date and time when the account is locked until
        /// </summary>
        public DateTime? LockedUntil { get; private set; }

        /// <summary>
        /// Whether the account is currently locked
        /// </summary>
        public bool IsLocked => LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;

        // Business methods

        /// <summary>
        /// Updates the user's email address
        /// </summary>
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidEntityException("Email is required");

            if (email.Length > 256)
                throw new InvalidEntityException("Email cannot exceed 256 characters");

            if (!IsValidEmail(email))
                throw new InvalidEntityException("Invalid email format");

            _email = email.ToLowerInvariant().Trim();
            IsEmailVerified = false; // Reset verification when email changes
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the user's first name
        /// </summary>
        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidEntityException("First name is required");

            if (firstName.Length > 50)
                throw new InvalidEntityException("First name cannot exceed 50 characters");

            _firstName = firstName.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the user's last name
        /// </summary>
        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidEntityException("Last name is required");

            if (lastName.Length > 50)
                throw new InvalidEntityException("Last name cannot exceed 50 characters");

            _lastName = lastName.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the user's phone number
        /// </summary>
        public void SetPhoneNumber(string phoneNumber)
        {
            if (phoneNumber != null && phoneNumber.Length > 20)
                throw new InvalidEntityException("Phone number cannot exceed 20 characters");

            _phoneNumber = phoneNumber?.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the user's password hash
        /// </summary>
        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentNullException(nameof(passwordHash));

            _passwordHash = passwordHash;
            MarkAsUpdated();
        }

        /// <summary>
        /// Changes the user's role
        /// </summary>
        public void ChangeRole(UserRole role)
        {
            if (role == UserRole.Guest)
                throw new BusinessRuleViolationException("Cannot set user role to Guest");

            Role = role;
            MarkAsUpdated();
        }

        /// <summary>
        /// Activates the user account
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            MarkAsUpdated();
        }

        /// <summary>
        /// Deactivates the user account
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            MarkAsUpdated();
        }

        /// <summary>
        /// Marks the email as verified
        /// </summary>
        public void VerifyEmail()
        {
            IsEmailVerified = true;
            MarkAsUpdated();
        }

        /// <summary>
        /// Records a successful login
        /// </summary>
        public void RecordSuccessfulLogin()
        {
            LastLoginDate = DateTime.UtcNow;
            FailedLoginAttempts = 0;
            LockedUntil = null;
            MarkAsUpdated();
        }

        /// <summary>
        /// Records a failed login attempt
        /// </summary>
        public void RecordFailedLogin(int maxAttempts = 5, int lockoutMinutes = 15)
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= maxAttempts)
            {
                LockedUntil = DateTime.UtcNow.AddMinutes(lockoutMinutes);
            }

            MarkAsUpdated();
        }

        /// <summary>
        /// Unlocks the user account
        /// </summary>
        public void Unlock()
        {
            FailedLoginAttempts = 0;
            LockedUntil = null;
            MarkAsUpdated();
        }

        /// <summary>
        /// Validates if the user can log in
        /// </summary>
        public bool CanLogin()
        {
            return IsActive && !IsLocked;
        }

        /// <summary>
        /// Checks if the user has a specific role
        /// </summary>
        public bool HasRole(UserRole role)
        {
            return Role == role;
        }

        /// <summary>
        /// Checks if the user is an administrator
        /// </summary>
        public bool IsAdministrator()
        {
            return Role == UserRole.Administrator;
        }

        /// <summary>
        /// Checks if the user is a manager or administrator
        /// </summary>
        public bool IsManagerOrHigher()
        {
            return Role == UserRole.Manager || Role == UserRole.Administrator;
        }

        // Helper methods

        /// <summary>
        /// Basic email validation
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
