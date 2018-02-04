using Abp.EntityFrameworkCore;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class MenuRepository : GalaxyRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
