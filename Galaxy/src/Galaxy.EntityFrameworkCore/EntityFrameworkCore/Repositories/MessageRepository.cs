using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
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
    }
}
