using Galaxy.Menus;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes.Requirement
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IMenuAppService menuAppService;
        public PermissionAuthorizationHandler(IMenuAppService _menuAppService)
        {
            menuAppService = _menuAppService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            throw new NotImplementedException();
        }

        /*
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(f => f.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        var list = await menuAppService.GetUserPermissions(int.Parse(userIdClaim.Value));
                        bool IsHavePermission = list.Any(p=>p.Code.Equals())
                        if (menuAppService.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
        }
        */
    }
}
