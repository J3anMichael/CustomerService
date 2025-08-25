using CustomerService.Application.Interfaces.Repositories;
using CustomerService.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Infrastructure.Persistence
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly ConcurrentDictionary<Guid, Clientes> _customers = new();

        public Task<Clientes> CreateAsync(Clientes customer)
        {
            _customers.TryAdd(customer.Id, customer);
            return Task.FromResult(customer);
        }

        public Task<Clientes> GetByIdAsync(Guid id)
        {
            _customers.TryGetValue(id, out var customer);
            return Task.FromResult(customer);
        }

        public Task<Clientes> GetByDocumentAsync(string document)
        {
            var customer = _customers.Values.FirstOrDefault(c => c.CPF == document);
            return Task.FromResult(customer);
        }

        public Task<Clientes> UpdateAsync(Clientes customer)
        {
            _customers.TryUpdate(customer.Id, customer, _customers[customer.Id]);
            return Task.FromResult(customer);
        }

        public Task<bool> ExistsByDocumentAsync(string document)
        {
            var exists = _customers.Values.Any(c => c.CPF == document);
            return Task.FromResult(exists);
        }
    }
}