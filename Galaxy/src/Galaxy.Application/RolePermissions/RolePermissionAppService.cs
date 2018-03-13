using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Entities;
using System.Threading.Tasks;
using Galaxy.IRepositories;

namespace Galaxy.RolePermissions
{
    /// <summary>
    /// 角色权限接口服务实现类
    /// </summary>
    public class RolePermissionAppService : ApplicationService, IRolePermissionAppService
    {
        private readonly IRolePermissionRepository rolePermissionRepository;
        /// <summary>
        /// 依赖注入初始化IRolePermissionRepository
        /// </summary>
        /// <param name="_rolePermissionRepository"></param>
        public RolePermissionAppService(IRolePermissionRepository _rolePermissionRepository)
        {
            rolePermissionRepository = _rolePermissionRepository;
        }

        /// <summary>
        /// 检查数据库是否存在此角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool CheckExistsRole(int roleId)
        {
            return rolePermissionRepository.CheckExistsRole(roleId);
        }

        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteRolePermission(int Id)
        {
            await rolePermissionRepository.DeleteAsync(Id);
        }

        /// <summary>
        /// 根据RoleId获取PermissionIds
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<string> GetPermissions(int roleId)
        {
            return await rolePermissionRepository.GetPermissions(roleId);
        }

        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RolePermission>> GetRolePermissions()
        {
            return await rolePermissionRepository.GetAllListAsync();
        }

        /// <summary>
        /// 新增角色权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostRolePermission(RolePermission entity)
        {
            await rolePermissionRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutRolePermission(RolePermission entity)
        {
            await rolePermissionRepository.UpdateAsync(entity);
        }
    }
}
