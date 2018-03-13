using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.IRepositories
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        bool CheckExistsRole(int roleId);

        Task<string> GetPermissions(int roleId);
    }
}
