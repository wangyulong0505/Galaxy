using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class RoleRepository : GalaxyRepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
