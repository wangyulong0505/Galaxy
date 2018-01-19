using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Messages
{
    public interface IMessageAppService : IApplicationService
    {
        Task<List<Message>> GetMessages();

        Task PutMessage(Message entity);

        Task PostMessage(Message entity);

        Task DeleteMessage(int Id);
    }
}
