using Application;
using Consumer.RabbitMQ.Consumers;
using Infra.DataBase.Postgres;
using MessageBroker.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationDependency();
builder.Services.AddDatabasePostgresDependency(builder.Configuration);
builder.Services.AddRabbitMqDependency(); ;
builder.Services.AddHostedService<MotorcycleCreatedConsumer>();

var host = builder.Build();
host.Run();
