
using Application.UseCases.Motorcycle.Events.Created;
using Domain.Adapters;
using MediatR;
using System.Text.Json;

namespace Consumer.RabbitMQ.Consumers
{
    public class MotorcycleCreatedConsumer : BackgroundService
    {
        private readonly IMessageBrokerAdapter _messageBrokerAdapter;
        //private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        // public MotorcycleCreatedConsumer(IMessageBrokerAdapter messageBrokerAdapter, IMediator mediator, IServiceProvider serviceProvider)
        public MotorcycleCreatedConsumer(IMessageBrokerAdapter messageBrokerAdapter, IServiceProvider serviceProvider)
        {
            _messageBrokerAdapter = messageBrokerAdapter;
            //_mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageBrokerAdapter.ConsumeQueue<MotorcycleCreatedEventRequest>("motorcycle-created", async (message) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(message);
                }
            });
        }
    }
}
