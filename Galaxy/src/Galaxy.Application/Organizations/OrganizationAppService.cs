using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Organizations
{
    /// <summary>
    /// 组织机构服务实现类
    /// </summary>
    public class OrganizationAppService : ApplicationService, IOrganizationAppService
    {
        private readonly IOrganizationRepository repository;
        /// <summary>
        /// 构造函数，依赖注入初始化
        /// </summary>
        /// <param name="_repository"></param>
        public OrganizationAppService(IOrganizationRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="Id"></param>
        public async Task DeleteOrganization(int Id)
        {
            await repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 根据Id获取组织信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Organization> GetOrganization(int Id)
        {
            return await repository.GetAsync(Id);
        }

        /// <summary>
        /// 获取组织列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Organization>> GetOrganizations()
        {
            return await repository.GetAllListAsync();
        }

        /// <summary>
        /// 新增组织
        /// </summary>
        /// <param name="entity"></param>
        public async Task PostOrganization(Organization entity)
        {
            await repository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改组织信息
        /// </summary>
        /// <param name="entity"></param>
        public async Task PutOrganization(Organization entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}
