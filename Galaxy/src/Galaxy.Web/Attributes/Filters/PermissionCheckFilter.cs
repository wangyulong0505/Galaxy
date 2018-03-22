using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Galaxy.Menus;
using Galaxy.Entities;
using Galaxy.Users;
using Galaxy.Web.Attributes.Attributes;

namespace Galaxy.Web.Attributes.Filters
{
    public class PermissionCheckFilter : IAuthorizationFilter
    {
        /* 页面按钮的权限可以有两种实现方式
         * 1、前端实现方式：
         *      这种方式要求：前端的按钮每个页面都是固定的，然后把固定的html封装成一个类（类似于table的分页），然后在页面Index加载的时候判断按钮的权限进行显示和隐藏
         *      前端页面变化比较大，所以很难固定前端页面的按钮位置
         * 2、后端过滤器实现：
         *      这种方式要求：可以根据需要继承IAuthorizationFilter或者ActionFilterAttribute，IAuthorizationFilter优先于ActionFilterAttribute
         *      在Filter中获取当前用户的权限然后根绝业务逻辑判断用户的增删改查权限，在页面按钮点击之后进行拦截
         *      后台的每个Controller的增删改查Action方法必须一致，这种的可以实现
         */
        /* 其实GetUserPermissions(int Id)这个方法可以修改为GetUserPermissions(string strUserName)
         * 毕竟开两次数据库连接的消耗比开一次数据库连接的消耗大
         */
        private readonly IMenuAppService menuAppService;
        private readonly IUserAppService userAppService;
        public PermissionCheckFilter(IMenuAppService _menuAppService, IUserAppService _userAppService)
        {
            menuAppService = _menuAppService;
            userAppService = _userAppService;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            /*
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.HttpContext.Request.QueryString == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            string strUserName = context.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(strUserName))
            {
                //跳转到登录页面
                context.HttpContext.Response.Redirect("Account/Login");
            }
            int Id = await userAppService.GetUserId(strUserName);
            string strPath = context.HttpContext.Request.Path.Value;
            //匿名访问
            //var actionAnonymous = context.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true) as IEnumerable<AllowAnonymousAttribute>;
            var attNames = context.ActionDescriptor.GetTypes().GetCustomAttributes(typeof(CustomerResourceAttribute), true) as IEnumerable<CustomerResourceAttribute>;
            //获取所有的权限
            List<Menu> menulist = await menuAppService.GetUserPermissions(Id);
            var joinResult = (from menuEntity in menulist
                              join attName in attNames on menuEntity.Code equals attName.ResourceName
                              select attName).Any();
            if (!joinResult)
            {
                //没有权限
            }
            else
            {
                return;
            }
            */
        }
    }
}
