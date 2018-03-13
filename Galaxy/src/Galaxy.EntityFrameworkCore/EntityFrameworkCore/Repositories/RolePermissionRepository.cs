using Abp.EntityFrameworkCore;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class RolePermissionRepository : GalaxyRepositoryBase<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public bool CheckExistsRole(int roleId)
        {
            return GetAll().SingleOrDefault(q => q.RoleId == roleId) != null; 
        }

        public async Task<string> GetPermissions(int roleId)
        {
            var result = await Task.Run(() => GetAll().SingleOrDefault(q => q.RoleId == roleId));
            if (result != null)
            {
                return result.PermissionIds;
            }
            return "";
        }
    }
}
