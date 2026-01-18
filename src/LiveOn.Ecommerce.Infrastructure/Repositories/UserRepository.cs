using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email.ToLower() && !x.IsDeleted);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.LastName == name && !x.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetByRoleASync(UserRole role)
        {
            return await _dbSet
                .Where(u => u.Role == role && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(string keywords)
        {
            var keywordsToLower = keywords.ToLower();

            return await _dbSet
                .Where(u => (u.FirstName.ToLower().Contains(keywordsToLower) ||
                            u.LastName.ToLower().Contains(keywordsToLower)) &&
                            !u.IsDeleted)
                .ToListAsync();
        }
    }
}
