using Galaxy.Menus;
using Galaxy.Permissions;
using Galaxy.Roles;
using Galaxy.Users;
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
        private readonly IUserAppService userAppService;
        public MenuViewComponent(IMenuAppService _menuAppService, IUserAppService _userAppService)
        {
            menuAppService = _menuAppService;
            userAppService = _userAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //由于一个用户可以有多个角色，所以需要根据用户Id获取所有角色的权限，然后求合集就是用户的所有权限
            string strUserName = User.Identity.Name;
            int Id = await userAppService.GetUserId(strUserName);
            List<Entities.Menu> menus = await menuAppService.GetUserPermissions(Id);

            return View(menus);
        }
    }
}
