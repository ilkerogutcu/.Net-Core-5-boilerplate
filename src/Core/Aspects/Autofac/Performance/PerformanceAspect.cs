using System.Diagnostics;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{

    /// <summary>
    ///     Performance Aspect
    /// </summary>
    public class PerformanceAspect : MethodInterception
    {
        private readonly int _interval;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceAspect"/> class.
        /// </summary>
        /// <param name="interval">Interval.</param>
        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        /// <summary>
        /// On before
        /// </summary>
        /// <param name="invocation">invocation.</param>
        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// On after
        /// </summary>
        /// <param name="invocation">invocation.</param>
        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance: {invocation.Method.DeclaringType?.FullName}.{invocation.Method.Name}" +
                                $"-->{_stopwatch.Elapsed.TotalSeconds}");
            }

            _stopwatch.Reset();
        }
    }
}