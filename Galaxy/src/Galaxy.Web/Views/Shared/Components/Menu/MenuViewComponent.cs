using Galaxy.Menus;
using Galaxy.Permissions;
using Galaxy.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Web.Components
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuAppService menuAppService;
        private readonly IPermissionAppService permissionAppService;
        private readonly IRoleAppService roleAppService;
        public MenuViewComponent(IMenuAppService _menuAppService, IPermissionAppService _permissionAppService, IRoleAppService _roleAppService)
        {
            menuAppService = _menuAppService;
            permissionAppService = _permissionAppService;
            roleAppService = _roleAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //由于一个用户可以有多个角色，所以需要根据用户Id获取所有角色的权限，然后求合集就是用户的所有权限
            var userId = HttpContext.Session.GetString("Id");
            List<Entities.Menu> menus = await menuAppService.GetMenus();

            return View(menus);
        }
    }
}
