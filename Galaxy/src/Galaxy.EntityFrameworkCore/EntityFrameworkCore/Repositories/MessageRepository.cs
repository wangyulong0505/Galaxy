using Abp.EntityFrameworkCore;
using Galaxy.Entities;
using Galaxy.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class MessageRepository : GalaxyRepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">0收件箱， 1发件箱， 2草稿箱， 3回收站</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strKey"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public List<Message> GetPagingMessage(int status, int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount)
        {
            IQueryable<Message> allQueryable = GetQueryableByKeys(strKey);
            itemCount = allQueryable.Where(q => q.MessageStatus == status).OrderBy(q => q.Id).ToList().Count;
            pageCount = itemCount % pageSize == 0 ? (itemCount / pageSize) : (itemCount / pageSize) + 1;

            //pageSize位-1时默认获取全部
            if (pageSize == -1)
            {
                return allQueryable.Where(q => q.MessageStatus == 0).OrderBy(q => q.Id).ToList();
            }

            return allQueryable.Where(q => q.MessageStatus == 0).OrderBy(q => q.Id).Skip(pageIndex - 1).Take(pageSize).ToList();
        }

        /// <summary>
        /// 根据查询关键字获取所有集合
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public IQueryable<Message> GetQueryableByKeys(string strKey)
        {
            if (!string.IsNullOrEmpty(strKey))
            {
                return GetAll().Where(q => q.Title.Contains(strKey)).OrderBy(q => q.Id);
            }
            else
            {
                return GetAll().OrderBy(q => q.Id);
            }
        }

        public void ExecuteSql()
        {
            using (GalaxyDbContext db = new GalaxyDbContext(new DbContextOptions<GalaxyDbContext>()))
            {
                //这里仅仅为了测试，具体还是以实际为准
                //SqlCommand只返回受影响的行数，所以用在新增，插入，修改，删除行，删除表等操作中
                //新增Table
                int addResult = db.Database.ExecuteSqlCommand(@"CREATE TABLE dbo.Test (id INT IDENTITY(1,1) PRIMARY KEY, Name VARCHAR(100), Age INT);");

                //插入
                SqlParameter Name1 = new SqlParameter("@Name", "王明");
                SqlParameter Age1 = new SqlParameter("@Age", 19);
                string strInsertSql = "INSERT INTO dbo.Test(Name, Age) VALUES(@Name, @Age)";
                int insertResult = db.Database.ExecuteSqlCommand(strInsertSql, new SqlParameter[] { Name1, Age1 });

                //修改， 使用sqlParameter防止注入攻击
                SqlParameter Name2 = new SqlParameter("@Name", "李磊");
                SqlParameter Age2 = new SqlParameter("@Age", 18);
                string strUpdateSql = "UPDATE dbo.Test SET Name=@Name, Age=@Age";
                int updateResult = db.Database.ExecuteSqlCommand(strUpdateSql, new SqlParameter[] { Name2, Age2 });

                //删除行
                SqlParameter Id1 = new SqlParameter("@Id", 1);
                string strDeleteSql = "DELETE FROM dbo.Test WHERE Id=@Id";
                int deleteResult = db.Database.ExecuteSqlCommand(strDeleteSql, new SqlParameter[] { Id1 });

                //删除Table
                string strDropSql = "Drop Table dbo.Test";
                int dropResult = db.Database.ExecuteSqlCommand(strDropSql);

                //SqlQuery返回查询的结果，所以用在查询操作中, SqlQuery在EFCore中没有，只能用FromSql, FromSql还能执行存储过程
                SqlParameter Id2 = new SqlParameter("@Id", 1);
                string strSelectSql = "SELECT * FROM dbo.Test WHERE Id=@Id";
                var selectResult = db.Set<Message>().FromSql(strSelectSql, new SqlParameter[] { Id2 });
            }
        }
    }
}
