using Abp.EntityFrameworkCore;
using Galaxy.Entities;
using Galaxy.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class MenuRepository : GalaxyRepositoryBase<Menu>, IMenuRepository
    {
        private readonly IDbContextProvider<GalaxyDbContext> provider;
        public MenuRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
            provider = dbContextProvider;
        }

        /// <summary>
        /// 获取当前用户的所有权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetUserPermissions(string strUserName)
        {
            //执行存储过程
            string strSql = "EXEC [dbo].[SP_GetUserPermissions] @UserName";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@UserName", strUserName) };
            return await Task.Run(() => provider.GetDbContext().Set<Menu>().FromSql(strSql, param).ToList());
        }
    }
}
