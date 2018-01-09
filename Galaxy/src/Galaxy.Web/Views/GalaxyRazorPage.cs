using Abp.AspNetCore.Mvc.Views;

namespace Galaxy.Web.Views
{
    public abstract class GalaxyRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected GalaxyRazorPage()
        {
            LocalizationSourceName = GalaxyConsts.LocalizationSourceName;
        }
    }
}
