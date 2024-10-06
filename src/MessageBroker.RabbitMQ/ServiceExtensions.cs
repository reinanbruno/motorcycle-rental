using Domain.Adapters;
using MessageBroker.RabbitMQ.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.RabbitMQ
{
    public static class ServiceExtensions
    {
        public static void AddRabbitMqDependency(
            this IServiceCollection services)
        {
            services.AddSingleton<IMessageBrokerAdapter, MessageBrokerAdapter>();
        }
    }
}
