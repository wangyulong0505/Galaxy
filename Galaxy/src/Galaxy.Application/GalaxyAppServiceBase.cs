using Abp.Application.Services;

namespace Galaxy
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class GalaxyAppServiceBase : ApplicationService
    {
        protected GalaxyAppServiceBase()
        {
            LocalizationSourceName = GalaxyConsts.LocalizationSourceName;
        }
    }
}