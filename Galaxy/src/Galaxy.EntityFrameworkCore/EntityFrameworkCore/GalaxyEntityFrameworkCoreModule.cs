using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Galaxy.EntityFrameworkCore
{
    [DependsOn(
        typeof(GalaxyCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class GalaxyEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GalaxyEntityFrameworkCoreModule).GetAssembly());
        }
    }
}