using CustomerService.Domain.Entities.Repositories;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Services;
using CustomerService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // Message Bus
            services.AddScoped<IMessageBus, MessageBus>();


            return services;
        }
    }
}