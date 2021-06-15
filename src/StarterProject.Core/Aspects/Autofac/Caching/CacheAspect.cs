using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using StarterProject.Core.CrossCuttingConcerns.Caching;
using StarterProject.Core.Utilities.Interceptors;
using StarterProject.Core.Utilities.IoC;

namespace StarterProject.Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private readonly ICacheManager _cacheManager;
        private readonly int _duration;

        public CacheAspect(int duration = 20)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            if (invocation.Method.ReflectedType is null) return;
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }

            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}