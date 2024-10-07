using Application.Middlewares;
using Application.Services;
using Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationDependency(
            this IServiceCollection services)
        {
            Assembly assembly = AppDomain.CurrentDomain.Load("Application");
            services.AddMediatR(m => m.RegisterServicesFromAssembly(assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationMiddleware<,>));
            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<IFileService, FileService>();
        }
    }
}
