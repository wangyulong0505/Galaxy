using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Entities;
using System.Threading.Tasks;
using Galaxy.IRepositories;

namespace Galaxy.Permissions
{
    /// <summary>
    /// 权限服务接口实现类
    /// </summary>
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IPermissionRepository permissionRepository;

        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        public PermissionAppService(IPermissionRepository _permissionRepository)
        {
            permissionRepository = _permissionRepository;
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeletePermission(int Id)
        {
            await permissionRepository.DeleteAsync(Id);
        }

        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Permission> GetPermissionDetail(int Id)
        {
            return await permissionRepository.GetAsync(Id);
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Permission>> GetPermissions()
        {
            return await permissionRepository.GetAllListAsync();
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostPermission(Permission entity)
        {
            await permissionRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutPermission(Permission entity)
        {
            await permissionRepository.UpdateAsync(entity);
        }
    }
}
