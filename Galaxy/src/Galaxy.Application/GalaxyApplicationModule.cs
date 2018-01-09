using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Galaxy
{
    [DependsOn(
        typeof(GalaxyCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class GalaxyApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GalaxyApplicationModule).GetAssembly());
        }
    }
}