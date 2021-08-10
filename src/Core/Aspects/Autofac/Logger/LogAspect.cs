using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core.Aspects.Autofac.Logger
{
    
    /// <summary>
    /// Log aspect.
    /// </summary>
    public class LogAspect : MethodInterception
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogAspect"/> class.
        /// </summary>
        /// <param name="loggerService">Logger service type.</param>
        public LogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new ArgumentException(AspectMessages.WrongLoggerType);
            }

            _loggerServiceBase = (LoggerServiceBase) ServiceTool.ServiceProvider.GetService(loggerService);
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        /// <summary>
        /// Logging on before
        /// </summary>
        /// <param name="invocation">invocation.</param>
        protected override void OnBefore(IInvocation invocation)
        {
            var result = GetLogDetail(invocation);
            _loggerServiceBase.Info($"OnBefore: {result}");
        }
        /// <summary>
        /// Logging on success
        /// </summary>
        /// <param name="invocation">invocation.</param>
        protected override void OnSuccess(IInvocation invocation)
        {
            var result = GetLogDetail(invocation);
            _loggerServiceBase.Info(
                $"OnSuccess Method Name: {result}");
        }

        /// <summary>
        /// Get parameters from invocation
        /// </summary>
        /// <param name="invocation">invocation.</param>
        /// <returns>Log parameter list.</returns>
        private static List<LogParameter> GetLogParameters(IInvocation invocation)
        {
            return invocation.Arguments.Select((t, i) => new LogParameter
            {
                Name = invocation.GetConcreteMethod().GetParameters()[i].Name ?? string.Empty, Value = t,
                Type = t.GetType().Name,
                ReturnValue = invocation.ReturnValue,
            }).ToList();
        }
        
        private string GetLogDetail(IInvocation invocation)
        {
            var logParameters = GetLogParameters(invocation);
            var logDetail = new LogDetail
            {
                FullName = invocation.Method.ToString(),
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?",
            };
            return JsonConvert.SerializeObject(logDetail, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
        }
    }
}