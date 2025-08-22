using CustomerService.Application.Interfaces.Repositories;
using CustomerService.Application.Interfaces.Services;
using CustomerService.Application.UseCases;
using CustomerService.Infrastructure.MessageBus;
using CustomerService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();

            // Message Bus
            services.AddSingleton<RabbitMQService>();
            services.AddSingleton<IMessageBusService, ResilientRabbitMQService>();

            // Use Cases
            services.AddScoped<CreateCustomerUseCase>();
            services.AddScoped<UpdateCustomerStatusUseCase>();

            return services;
        }
    }
}