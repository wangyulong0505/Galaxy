using Galaxy.Menus;
using Galaxy.Permissions;
using Galaxy.Roles;
using Galaxy.Users;
using Galaxy.Web.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Web.Components
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuAppService menuAppService;
        private readonly IUserAppService userAppService;
        private readonly SuperAdmin super;
        public MenuViewComponent(IMenuAppService _menuAppService, IUserAppService _userAppService, IOptions<SuperAdmin> options)
        {
            menuAppService = _menuAppService;
            userAppService = _userAppService;
            super = options.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //由于一个用户可以有多个角色，所以需要根据用户Id获取所有角色的权限，然后求合集就是用户的所有权限
            string strUserName = User.Identity.Name;
            List<Entities.Menu> menus = new List<Entities.Menu>();
            if (strUserName == super.UserName)
            {
                menus = await menuAppService.GetMenus();
            }
            else
            {
                int Id = await userAppService.GetUserId(strUserName);
                menus = await menuAppService.GetUserPermissions(Id);
            }

            return View(menus);
        }
    }
}
