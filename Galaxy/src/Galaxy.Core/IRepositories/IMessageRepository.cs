using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.IRepositories
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
