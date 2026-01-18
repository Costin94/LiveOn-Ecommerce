using LiveOn.Ecommerce.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LiveOn.Ecommerce.Infrastructure.Data.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            HasKey(u => u.Id);

            Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // BaseEntity properties
            Property(u => u.CreatedAt).IsRequired();
            Property(u => u.UpdatedAt).IsOptional();
            Property(u => u.IsDeleted).IsRequired();

            // User properties
            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            Property(u => u.PhoneNumber)
                .IsOptional()
                .HasMaxLength(20);

            Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            Property(u => u.Role)
                .IsRequired();

            Property(u => u.IsActive)
                .IsRequired();

            Property(u => u.IsEmailVerified)
                .IsRequired();

            Property(u => u.RegistrationDate)
                .IsRequired();

            Property(u => u.LastLoginDate)
                .IsOptional();

            Property(u => u.FailedLoginAttempts)
                .IsRequired();

            Property(u => u.LockedUntil)
                .IsOptional();

            // Ignore computed properties
            Ignore(u => u.FullName);
            Ignore(u => u.IsLocked);
        }
    }
}
