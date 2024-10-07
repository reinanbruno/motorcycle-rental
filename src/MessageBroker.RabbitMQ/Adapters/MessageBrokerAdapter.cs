using Domain.Adapters;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Text.Json;

namespace MessageBroker.RabbitMQ.Services
{
    public class MessageBrokerAdapter : IMessageBrokerAdapter
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;

        public MessageBrokerAdapter(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RabbitMQ");
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
            };
            Connect();
        }

        private void Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            _model = _connectionFactory.CreateConnection().CreateModel();

            _connection.ConnectionShutdown += Reconnect;
        }

        private void Reconnect(object sender, ShutdownEventArgs e)
        {
            while (true)
            {
                try
                {
                    Connect();
                    Log.Information("Reconectado ao RabbitMQ com sucesso.");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Erro ao tentar se conectar no RabbitMQ");
                    Task.Delay(5000);
                }
            }
        }

        public void ConsumeQueue<T>(string queueName, Func<T, Task> handler)
        {
            _model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_model);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var eventMessage = JsonSerializer.Deserialize<T>(message);

                await handler(eventMessage);
            };

            _model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void PublishQueue<T>(string queueName, T message)
        {
            _model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var messageText = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageText);
            _model.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
