using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Users
{
    public interface IUserAppService : IApplicationService
    {
        List<RegisterUserDto> GetRegisterUsers();

        List<User> GetUsers();

        int GetLoginStatus(string usernameOrEmailAddress, string password);
    }
}
