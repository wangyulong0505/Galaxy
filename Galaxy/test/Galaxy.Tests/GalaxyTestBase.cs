using System;
using System.Threading.Tasks;
using Abp.TestBase;
using Galaxy.EntityFrameworkCore;
using Galaxy.Tests.TestDatas;

namespace Galaxy.Tests
{
    public class GalaxyTestBase : AbpIntegratedTestBase<GalaxyTestModule>
    {
        public GalaxyTestBase()
        {
            UsingDbContext(context => new TestDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<GalaxyDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<GalaxyDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<GalaxyDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<GalaxyDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<GalaxyDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<GalaxyDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<GalaxyDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<GalaxyDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
