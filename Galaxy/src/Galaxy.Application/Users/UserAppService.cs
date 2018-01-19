using Abp.Application.Services;
using AutoMapper;
using Galaxy.Entities;
using Galaxy.IRepositories;
using Galaxy.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository userReposiory;
        public UserAppService(IUserRepository _userReposiory)
        {
            userReposiory = _userReposiory;
        }

        /// <summary>
        /// 验证登录的状态 0：成功， 1：用户名或邮箱不存在， 2：密码错误， 3：用户名或邮箱为空
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int GetLoginStatus(string usernameOrEmailAddress, string password)
        {
            return userReposiory.GetLoginStatus(usernameOrEmailAddress, password);
        }

        public List<RegisterUserDto> GetRegisterUsers()
        {
            List<User> userList = userReposiory.GetAll().ToList();
            //这里不能调用Initialize了，因为Dto有特性MapperTo了，Mapper只能初始化一次
            //Mapper.Initialize(m => m.CreateMap<User, RegisterUserDto>());  
            List<RegisterUserDto> list = Mapper.Map<List<User>, List<RegisterUserDto>>(userList);
            return list;
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
