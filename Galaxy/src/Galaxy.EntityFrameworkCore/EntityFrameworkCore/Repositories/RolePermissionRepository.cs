using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class RolePermissionRepository : GalaxyRepositoryBase<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
