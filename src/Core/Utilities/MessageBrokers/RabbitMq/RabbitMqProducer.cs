using System.Threading.Tasks;
using Core.Aspects.Autofac.Exception;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Settings;
using Core.Utilities.IoC;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly IBus _bus;

        private static readonly RabbitMqSettings RabbitMqSettings = ServiceTool.ServiceProvider
            .GetService<IConfiguration>()
            ?.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

        public RabbitMqProducer(IBus bus)
        {
            _bus = bus;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task Publish(ProducerModel producerModel)
        {
            var sendToUri = new System.Uri($"{RabbitMqSettings.RabbitMqUri}{producerModel.QueueName}");
            var endPoint = await _bus.GetSendEndpoint(sendToUri);
            await endPoint.Send(producerModel.Model);
        }
    }
}