#region

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#endregion

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var message = "";
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails(message);
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}