using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Galaxy.Configuration;
using Galaxy.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Galaxy.Web.Startup
{
    [DependsOn(
        typeof(GalaxyApplicationModule), 
        typeof(GalaxyEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class GalaxyWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public GalaxyWebModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(GalaxyConsts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<GalaxyNavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(GalaxyApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GalaxyWebModule).GetAssembly());
        }
    }
}