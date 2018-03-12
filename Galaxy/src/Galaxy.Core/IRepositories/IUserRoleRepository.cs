using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.IRepositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<List<User>> GetExcludeUsersByRoleId(int RoleId);

        Task<List<User>> GetUsersByRoleId(int RoleId);

        Task RemoveUserRole(UserRole entity);
    }
}
