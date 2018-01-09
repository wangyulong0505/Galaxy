using System.Reflection;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Galaxy.Web.Startup;

namespace Galaxy.Web.Tests
{
    [DependsOn(
        typeof(GalaxyWebModule),
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class GalaxyWebTestModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GalaxyWebTestModule).GetAssembly());
        }
    }
}