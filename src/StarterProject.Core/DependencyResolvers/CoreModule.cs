using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StarterProject.Core.CrossCuttingConcerns.Caching;
using StarterProject.Core.CrossCuttingConcerns.Caching.Microsoft;
using StarterProject.Core.Utilities.IoC;

namespace StarterProject.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}