using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<List<Document>> GetDocuments();

        Task PutDocument(Document entity);

        Task PostDocument(Document entity);

        Task DeleteDocument(int Id);

        List<Document> GetPagingDocuments(int pageIndex, int pageSize, out int pageCount, out int itemCount);
    }
}
