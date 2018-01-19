﻿using System.Collections.Generic;
using Abp.Application.Services;
using Galaxy.Entities;
using System.Threading.Tasks;
using Galaxy.IRepositories;

namespace Galaxy.Messages
{
    public class MessageAppService : ApplicationService, IMessageAppService
    {
        private readonly IMessageRepository repository;
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
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetMessages()
        {
            return await repository.GetAllListAsync();
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
