using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.UserRoles
{
    /// <summary>
    /// 用户角色服务接口
    /// </summary>
    public interface IUserRoleAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<UserRole>> GetUserRoles();

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutUserRoles(UserRole entity);

        /// <summary>
        /// 新增用户角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostUserRole(UserRole entity);

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteUserRole(int Id);

        /// <summary>
        /// 根据RoleId获取所有的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<List<User>> GetUsersByRoleId(int RoleId);

        /// <summary>
        /// 根据RoleId获取所有不属于此角色的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<List<User>> GetExcludeUsersByRoleId(int RoleId);
    }
}
