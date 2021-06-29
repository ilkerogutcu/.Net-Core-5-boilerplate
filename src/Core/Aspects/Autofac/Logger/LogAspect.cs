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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core.Aspects.Autofac.Logger
{
    public class LogAspect : MethodInterception //Aspect
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        public LogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
                throw new ArgumentException(AspectMessages.WrongLoggerType);
            _loggerServiceBase = (LoggerServiceBase) ServiceTool.ServiceProvider.GetService(loggerService);
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        // protected override void OnBefore(IInvocation invocation)
        // {
        //     var result = GetLogDetail(invocation);
        //     _loggerServiceBase.Info($"OnBefore: {result}");
        // }

        protected override void OnException(IInvocation invocation, Exception e)
        {
            var result = GetLogDetailWithException(invocation, e);
            _loggerServiceBase.Error(
                $"OnException {result}");
        }

        // protected override void OnSuccess(IInvocation invocation)
        // {
        //     var result = GetLogDetail(invocation);
        //     _loggerServiceBase.Info(
        //         $"OnSuccess Method Name: {result}");
        // }

        // protected override void OnAfter(IInvocation invocation)
        //  {
        //     you can use like others
        //  }

        private string GetLogDetailWithException(IInvocation invocation, Exception e)
        {
            var logParameters = GetLogParameters(invocation);
            var logDetail = new LogDetailWithException
            {
                FullName = invocation.Method.ToString(),
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?",
                ExceptionMessage = e.Message
            };
            return JsonConvert.SerializeObject(logDetail, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                });
        }

        private string GetLogDetail(IInvocation invocation)
        {
            var logParameters = GetLogParameters(invocation);
            var logDetail = new LogDetail
            {
                FullName = invocation.Method.ToString(),
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?"
            };
            return JsonConvert.SerializeObject(logDetail, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private static List<LogParameter> GetLogParameters(IInvocation invocation)
        {
            return invocation.Arguments.Select((t, i) => new LogParameter
            {
                Name = invocation.GetConcreteMethod().GetParameters()[i].Name ?? string.Empty, Value = t,
                Type = t.GetType().Name,
                ReturnValue = invocation.ReturnValue
            }).ToList();
        }

        private static string GetProjectName()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            return configuration?.GetSection("SeriLogConfigurations:ProjectName").Value;
        }
    }
}