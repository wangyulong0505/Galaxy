using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using System.Linq;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class DocumentRepository : GalaxyRepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public List<Document> GetPagingDocuments(int pageIndex, int pageSize, out int pageCount, out int itemCount)
        {
            List<Document> allList = GetAllList();
            itemCount = allList.Count;
            pageCount = itemCount % pageSize == 0 ? (itemCount / pageSize) : (itemCount / pageSize) + 1;

            //pageSize位-1时默认获取全部
            if (pageSize == -1)
            {
                return GetAll().OrderBy(q => q.Id).ToList();
            }

            return GetAll().OrderBy(q => q.Id).Skip(pageIndex - 1).Take(pageSize).ToList();
        }
    }
}
