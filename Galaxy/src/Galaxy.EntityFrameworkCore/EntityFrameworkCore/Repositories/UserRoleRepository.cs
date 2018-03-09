using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class UserRoleRepository : GalaxyRepositoryBase<UserRole>, IUserRoleRepository
    {
        private readonly IDbContextProvider<GalaxyDbContext> provider;
        public UserRoleRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
            provider = dbContextProvider;
        }

        /// <summary>
        /// 根据RoleId获取所有不属于此角色的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetExcludeUsersByRoleId(int RoleId)
        {
            string strQuerySql = "SELECT u.Id, u.Name, u.UserName FROM dbo.Users u LEFT JOIN dbo.UserRoles r ON r.UserId = u.Id WHERE NOT EXISTS(SELECT 1 FROM dbo.UserRoles WHERE RoleId = @RoleId)";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@RoleId", RoleId) };
            return await Task.Run(() => provider.GetDbContext().Set<User>().Select(q => new User { Id = q.Id, Name = q.Name, UserName = q.UserName }).FromSql(strQuerySql, param).ToList());
        }

        /// <summary>
        /// 通过RoleId获取关联的Users集合
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersByRoleId(int RoleId)
        {
            /* 两种
             * 1、先根据RoleId获取UserRole中的UserId列表，然后和User表连接，筛选出需要的数据返回（推荐）
             * 2、先UserRole和User表连接，然后通过RoleId筛选出需要的数据返回
             * 使用Join默认生成InnerJoin的查询，所以还是直接使用sql语句
             */
            /*
            var result = db.Users.Join(db.UserRoles, a => a.Id, u => u.UserId, (a, u) => new { a.Id, a.Name, a.UserName, u.RoleId });
            return await Task.Run(() => result.Where(q => q.RoleId == RoleId).Select(q => new User() { Id = q.Id, Name = q.Name, UserName = q.UserName }).ToList());
            */
            string strQuerySql = "SELECT u.Id, u.Name, u.UserName FROM dbo.Users u LEFT JOIN dbo.UserRoles r ON r.UserId = u.Id WHERE r.RoleId = @RoleId ";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@RoleId", RoleId) };
            return await Task.Run(() => provider.GetDbContext().Set<User>().Select(q=> new User { Id = q.Id, Name = q.Name, UserName = q.UserName }).FromSql(strQuerySql, param).ToList());
        }
    }
}
