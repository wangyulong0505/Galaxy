using Abp.Modules;
using Abp.Reflection.Extensions;
using Galaxy.Localization;

namespace Galaxy
{
    public class GalaxyCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            GalaxyLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GalaxyCoreModule).GetAssembly());
        }
    }
}