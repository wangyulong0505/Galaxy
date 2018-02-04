using Abp.Application.Services;
using Galaxy.Entities;
using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Menus
{
    /// <summary>
    /// 菜单服务接口实现类
    /// </summary>
    public class MenuAppService : ApplicationService, IMenuAppService
    {
        private readonly IMenuRepository repository;
        /// <summary>
        /// 构造函数初始化和依赖注入
        /// </summary>
        /// <param name="_repository"></param>
        public MenuAppService(IMenuRepository _repository)
        {
            repository = _repository;
        }
        
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteMenu(int Id)
        {
            await repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuDetail(int Id)
        {
            return await repository.GetAsync(Id);
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenus()
        {
            return await repository.GetAllListAsync();
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PostMenu(Menu entity)
        {
            await repository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task PutMenu(Menu entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}
