using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Users
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取注册用户
        /// </summary>
        /// <returns></returns>
        List<RegisterUserDto> GetRegisterUsers();

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<User> GetUsers();

        /// <summary>
        /// 获取登录状态 0：成功， 1：用户名或邮箱不存在， 2：密码错误， 3：用户名或邮箱为空
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        int GetLoginStatus(string usernameOrEmailAddress, string password);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutUser(User entity);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostUser(User entity);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteUser(int Id);

        /// <summary>
        /// 分页获取用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strKey"></param>
        /// <param name="pageCount"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        List<User> GetPagingUsers(int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount);

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<User> GetUserDetail(int Id);

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        Task<int> GetUserId(string strUserName);
    }
}
