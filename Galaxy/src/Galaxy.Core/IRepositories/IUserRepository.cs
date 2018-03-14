using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.IRepositories
{
    public interface IUserRepository: IRepository<User>
    {
        int GetLoginStatus(string usernameOrEmailAddress, string password);

        List<User> GetPagingUsers(int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount);

        List<User> GetUsers();

        Task<int> GetUserId(string strUserName);
    }
}
