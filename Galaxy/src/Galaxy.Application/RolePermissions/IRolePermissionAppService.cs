using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.RolePermissions
{
    /// <summary>
    /// 角色权限服务接口
    /// </summary>
    public interface IRolePermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <returns></returns>
        Task<List<RolePermission>> GetRolePermissions();

        /// <summary>
        /// 新增角色权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostRolePermission(RolePermission entity);

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutRolePermission(RolePermission entity);

        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteRolePermission(int Id);
    }
}
