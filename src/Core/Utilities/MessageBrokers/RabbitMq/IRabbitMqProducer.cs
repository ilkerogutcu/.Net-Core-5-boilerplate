using System.Threading.Tasks;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public interface IRabbitMqProducer
    {
        Task Publish(ProducerModel producerModel);
    }
}