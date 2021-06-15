using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using StarterProject.Core.CrossCuttingConcerns.Caching;
using StarterProject.Core.Utilities.Interceptors;
using StarterProject.Core.Utilities.IoC;

namespace StarterProject.Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private readonly ICacheManager _cacheManager;
        private readonly string _pattern;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}