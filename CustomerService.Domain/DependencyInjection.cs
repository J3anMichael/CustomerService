using CustomerService.Domain.Entities.Repositories;
using CustomerService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainDependencyInjection(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IClienteRepository, ClienteRepository>();


            return services;
        }
    }
}