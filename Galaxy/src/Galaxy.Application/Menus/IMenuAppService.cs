using Abp.Application.Services;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Menus
{
    /// <summary>
    /// 菜单服务接口
    /// </summary>
    public interface IMenuAppService: IApplicationService
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetMenus();

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PostMenu(Menu entity);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task PutMenu(Menu entity);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteMenu(int Id);

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Menu> GetMenuDetail(int Id);

        /// <summary>
        /// 获取当前用户的所有权限
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        Task<List<Menu>> GetUserPermissions(string strUserName);
    }
}
