using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.IRepositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        List<Document> GetPagingDocuments(int pageIndex, int pageSize, out int pageCount, out int itemCount);
    }
}
