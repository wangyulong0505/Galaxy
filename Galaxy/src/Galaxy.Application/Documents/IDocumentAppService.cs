using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Documents
{
    /// <summary>
    /// Document服务接口
    /// </summary>
    public interface IDocumentAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有文档
        /// </summary>
        /// <returns></returns>
        Task<List<Document>> GetDocuments();

        /// <summary>
        /// 修改文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutDocument(Document entity);

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostDocument(Document entity);

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteDocument(int Id);

        /// <summary>
        /// 分页获取文档
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        List<Document> GetPagingDocuments(int pageIndex, int pageSize, out int pageCount, out int itemCount);

        /// <summary>
        /// 获取文档详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Document> GetDocumentDetail(int Id);
    }
}
