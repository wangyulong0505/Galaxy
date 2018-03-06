using System.Collections.Generic;
using Abp.Application.Services;
using Galaxy.Entities;
using System.Threading.Tasks;
using Galaxy.IRepositories;
using System.Linq;

namespace Galaxy.Messages
{
    /// <summary>
    /// 消息服务实现类
    /// </summary>
    public class MessageAppService : ApplicationService, IMessageAppService
    {
        private readonly IMessageRepository repository;
        /// <summary>
        /// 构造函数，依赖注入初始化
        /// </summary>
        /// <param name="_repository"></param>
        public MessageAppService(IMessageRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteMessage(int Id)
        {
            await repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 获取草稿箱信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetDraftMessage()
        {
            List<Message> allMessage = await GetMessages();
            return allMessage.Where(q => q.MessageStatus == 0).ToList();
        }

        /// <summary>
        /// 获取收件箱信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetInboxMessage()
        {
            List<Message> allMessage = await GetMessages();
            return allMessage.Where(q => q.MessageStatus == 2).ToList();
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetMessages()
        {
            return await repository.GetAllListAsync();
        }

        /// <summary>
        /// 获取发件箱信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetOutboxMessage()
        {
            List<Message> allMessage = await GetMessages();
            return allMessage.Where(q => q.MessageStatus == 1).ToList();
        }

        /// <summary>
        /// 分页获取草稿箱信息
        /// </summary>
        /// <param name="status">消息状态：0收件箱， 1发件箱， 2草稿箱， 3回收站</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="strKey">查询关键字</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="itemCount">总记录数</param>
        /// <returns></returns>
        public List<Message> GetPagingMessage(int status, int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount)
        {
            return repository.GetPagingMessage(status, pageIndex, pageSize, strKey, out pageCount, out itemCount);
        }

        /// <summary>
        /// 获取垃圾箱信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetTrashMessage()
        {
            List<Message> allMessage = await GetMessages();
            return allMessage.Where(q => q.MessageStatus == 3).ToList();
        }

        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="entity"></param>
        public async Task PostMessage(Message entity)
        {
            await repository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改消息
        /// </summary>
        /// <param name="entity"></param>
        public async Task PutMessage(Message entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}
