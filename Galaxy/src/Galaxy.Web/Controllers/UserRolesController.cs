using Galaxy.Entities;
using Galaxy.Roles;
using Galaxy.UserRoles;
using Galaxy.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Web.Controllers
{
    public class UserRolesController : GalaxyControllerBase
    {
        private readonly IRoleAppService roleService;
        private readonly IUserAppService userService;
        private readonly IUserRoleAppService userRoleAppService;
        public UserRolesController(IRoleAppService _roleService, IUserAppService _userService, IUserRoleAppService _userRoleAppService)
        {
            roleService = _roleService;
            userService = _userService;
            userRoleAppService = _userRoleAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">RoleId</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int Id)
        {
            //获取所有的数据
            List<User> bindList = await userRoleAppService.GetUsersByRoleId(Id);
            List<User> unbindList = await userRoleAppService.GetExcludeUsersByRoleId(Id);

            ViewData["BindList"] = bindList;
            ViewData["UnbindList"] = unbindList;
            return View();
        }
    }
}