using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Permissions
{
    /// <summary>
    /// 权限服务接口
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        Task<List<Permission>> GetPermissions();

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostPermission(Permission entity);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutPermission(Permission entity);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeletePermission(int Id);

        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Permission> GetPermissionDetail(int Id);
    }
}
