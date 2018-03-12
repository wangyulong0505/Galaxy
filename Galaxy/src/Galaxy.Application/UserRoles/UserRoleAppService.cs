using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;

namespace Galaxy.UserRoles
{
    /// <summary>
    /// 用户角色接口服务实现类
    /// </summary>
    public class UserRoleAppService : ApplicationService, IUserRoleAppService
    {
        private readonly IUserRoleRepository userRoleRepository;
        /// <summary>
        /// 依赖注入初始化IUserRoleRepository
        /// </summary>
        /// <param name="_userRoleRepository"></param>
        public UserRoleAppService(IUserRoleRepository _userRoleRepository)
        {
            userRoleRepository = _userRoleRepository;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteUserRole(int Id)
        {
            await userRoleRepository.DeleteAsync(Id);
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task RemoveUserRole(UserRole entity)
        {
            await userRoleRepository.RemoveUserRole(entity);
        }

        /// <summary>
        /// 根据RoleId获取所有不属于此角色的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetExcludeUsersByRoleId(int RoleId)
        {
            return await userRoleRepository.GetExcludeUsersByRoleId(RoleId);
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRole>> GetUserRoles()
        {
            return await userRoleRepository.GetAllListAsync();
        }

        /// <summary>
        /// 根据RoleId获取所有的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersByRoleId(int RoleId)
        {
            return await userRoleRepository.GetUsersByRoleId(RoleId);
        }

        /// <summary>
        /// 新增用户角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostUserRole(UserRole entity)
        {
            await userRoleRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutUserRoles(UserRole entity)
        {
            await userRoleRepository.UpdateAsync(entity);
        }
    }
}
