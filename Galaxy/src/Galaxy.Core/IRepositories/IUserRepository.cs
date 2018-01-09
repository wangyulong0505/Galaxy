using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.IRepositories
{
    public interface IUserRepository: IRepository<User>
    {
        int GetLoginStatus(string usernameOrEmailAddress, string password);
    }
}
