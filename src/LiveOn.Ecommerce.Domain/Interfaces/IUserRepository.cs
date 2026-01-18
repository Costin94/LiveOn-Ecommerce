using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByNameAsync(string name);

        Task<User> GetByEmailAsync(string email);

        Task<IEnumerable<User>> SearchAsync(string keywords);

        Task<IEnumerable<User>> GetActiveUsersAsync();

        Task<IEnumerable<User>> GetByRoleASync(UserRole role);
    }
}
