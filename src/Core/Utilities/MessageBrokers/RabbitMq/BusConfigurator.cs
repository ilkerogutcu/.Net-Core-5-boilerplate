using Core.Settings;
using Core.Utilities.IoC;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public static class BusConfigurator
    {
        private static IBusControl _bus;
        public static IBusControl Bus => _bus ??= CreateUsingRabbitMq();

        private static readonly RabbitMqSettings RabbitMqSettings = ServiceTool.ServiceProvider.GetService<IConfiguration>()
            ?.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
        private static IBusControl CreateUsingRabbitMq()
        {
            var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(new System.Uri(RabbitMqSettings.RabbitMqUri), h =>
                {
                    h.Username(RabbitMqSettings.Username);
                    h.Password(RabbitMqSettings.Password);
                });
            });
            return bus;
        }
    }
}