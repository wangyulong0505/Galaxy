using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using System.Linq;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class OrganizationRepository : GalaxyRepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Organization GetOrganizationById(int Id)
        {
            return GetAll().SingleOrDefault(q => q.Id == Id);
        }
    }
}
