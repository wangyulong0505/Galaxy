using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Galaxy.Web.Controllers
{
    [Authorize]
    public abstract class GalaxyControllerBase: AbpController
    {
        protected GalaxyControllerBase()
        {
            LocalizationSourceName = GalaxyConsts.LocalizationSourceName;
        }
    }
}