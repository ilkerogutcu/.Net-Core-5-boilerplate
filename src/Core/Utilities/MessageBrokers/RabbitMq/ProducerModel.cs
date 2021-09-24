namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public class ProducerModel
    {
        public object Model { get; set; }
        public string QueueName { get; set; }
    }
}