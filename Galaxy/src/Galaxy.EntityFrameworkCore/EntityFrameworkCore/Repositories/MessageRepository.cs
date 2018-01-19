using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class MessageRepository : GalaxyRepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
