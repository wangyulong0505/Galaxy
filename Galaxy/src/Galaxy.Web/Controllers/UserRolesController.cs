using Galaxy.Roles;
using Galaxy.UserRoles;
using Galaxy.Users;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(int Id)
        {
            return View();
        }
    }
}