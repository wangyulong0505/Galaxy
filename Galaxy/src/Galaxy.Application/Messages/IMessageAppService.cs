using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Messages
{
    public interface IMessageAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetMessages();

        /// <summary>
        /// 修改/编辑消息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutMessage(Message entity);

        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostMessage(Message entity);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteMessage(int Id);

        /// <summary>
        /// 获取收件箱信息
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetInboxMessage();

        /// <summary>
        /// 获取发件箱信息
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetOutboxMessage();

        /// <summary>
        /// 获取草稿箱信息
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetDraftMessage();

        /// <summary>
        /// 获取垃圾箱信息
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetTrashMessage();

        /// <summary>
        /// 分页获取收件箱信息
        /// </summary>
        /// <param name="status">消息状态：0收件箱， 1发件箱， 2草稿箱， 3回收站</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="strKey">查询关键字</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="itemCount">总记录数</param>
        /// <returns></returns>
        List<Message> GetPagingMessage(int status, int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount);
    }
}
