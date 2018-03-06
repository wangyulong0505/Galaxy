using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.IRepositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staus">0收件箱， 1发件箱， 2草稿箱， 3回收站</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strKey"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        List<Message> GetPagingMessage(int staus, int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount);
    }
}
