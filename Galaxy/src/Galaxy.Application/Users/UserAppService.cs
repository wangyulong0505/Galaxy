using Abp.Application.Services;
using AutoMapper;
using Galaxy.Entities;
using Galaxy.IRepositories;
using Galaxy.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Users
{
    /// <summary>
    /// 用户服务实现类
    /// </summary>
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository userReposiory;
        /// <summary>
        /// 构造函数，依赖注入初始化
        /// </summary>
        /// <param name="_userReposiory"></param>
        public UserAppService(IUserRepository _userReposiory)
        {
            userReposiory = _userReposiory;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteUser(int Id)
        {
            await userReposiory.DeleteAsync(Id);
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

        /// <summary>
        /// 分页获取用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strKey"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public List<User> GetPagingUsers(int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount)
        {
            return userReposiory.GetPagingUsers(pageIndex, pageSize, strKey, out pageCount, out itemCount);
        }

        /// <summary>
        /// 获取注册用户
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserDto> GetRegisterUsers()
        {
            List<User> userList = userReposiory.GetAll().ToList();
            //这里不能调用Initialize了，因为Dto有特性MapperTo了，Mapper只能初始化一次
            //Mapper.Initialize(m => m.CreateMap<User, RegisterUserDto>());  
            List<RegisterUserDto> list = Mapper.Map<List<User>, List<RegisterUserDto>>(userList);
            return list;
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetUserDetail(int Id)
        {
            return await userReposiory.GetAsync(Id);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostUser(User entity)
        {
            await userReposiory.InsertAsync(entity);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutUser(User entity)
        {
            await userReposiory.UpdateAsync(entity);
        }

        /// <summary>
        /// 异步获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return userReposiory.GetUsers();
        }
    }
}
