using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;
using Infra.DataBase.Postgres.Repositories;

namespace Infra.DataBase.Postgres
{
    public static class ServiceExtensions
    {
        public static void AddDatabasePostgresDependency(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            services.AddDbContext<MotorcycleRentalDbContext>(opt => opt.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
            services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
        }

        public static void CreateDatabasePostgres(this WebApplication app)
        {
            var serviceScope = app.Services.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<MotorcycleRentalDbContext>();
            dataContext?.Database.EnsureCreated();
        }
    }
}
