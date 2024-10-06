namespace Domain.Adapters
{
    public interface IMessageBrokerAdapter
    {
        void PublishQueue<T>(string queueName, T message);
        void ConsumeQueue<T>(string queueName, Func<T, Task> handler);
    }
}
