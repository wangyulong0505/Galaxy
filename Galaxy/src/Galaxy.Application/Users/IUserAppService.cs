using Abp.Application.Services;
using Galaxy.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Users
{
    public interface IUserAppService : IApplicationService
    {
        List<RegisterUserDto> GetUsers();

        int GetLoginStatus(string usernameOrEmailAddress, string password);
    }
}
