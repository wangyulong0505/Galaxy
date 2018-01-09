using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System.Linq;

namespace Galaxy.Organization
{
    public class OrganizationAppService : ApplicationService, IOrganizationAppService
    {
        private readonly IOrganizationRepository repository;
        public OrganizationAppService(IOrganizationRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteOrganization(Entities.Organization entity)
        {
            repository.Delete(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Entities.Organization GetOrganizationById(int Id)
        {
            return repository.GetOrganizationById(Id);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entities.Organization> GetOrganizationList()
        {
            return repository.GetAll().ToList();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public void PostOrganization(Entities.Organization entity)
        {
            repository.Insert(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public void PutOrganization(Entities.Organization entity)
        {
            repository.Update(entity);
        }
    }
}
