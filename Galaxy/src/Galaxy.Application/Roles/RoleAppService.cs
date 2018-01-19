using System.Collections.Generic;
using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Roles
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRoleRepository repository;
        public RoleAppService(IRoleRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Id"></param>
        public async Task DeleteRole(int Id)
        {
            await repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            return await repository.GetAllListAsync();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity"></param>
        public async Task PostRole(Role entity)
        {
            await repository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="entity"></param>
        public async Task PutRole(Role entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}
