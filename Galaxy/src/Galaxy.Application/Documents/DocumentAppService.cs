using System.Collections.Generic;
using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System.Threading.Tasks;

namespace Galaxy.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IDocumentRepository repository;
        public DocumentAppService(IDocumentRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteDocument(int Id)
        {
            await repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Document> GetDocumentDetail(int Id)
        {
            return await repository.GetAsync(Id);
        }

        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Document>> GetDocuments()
        {
            return await repository.GetAllListAsync();
        }

        /// <summary>
        /// 分页获取文档数据列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public List<Document> GetPagingDocuments(int pageIndex, int pageSize, out int pageCount, out int itemCount)
        {
            return repository.GetPagingDocuments(pageIndex, pageSize, out pageCount, out itemCount);
        }

        /// <summary>
        /// 新增文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostDocument(Document entity)
        {
            await repository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutDocument(Document entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}
