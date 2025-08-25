using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Domain.Entities.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ConcurrentDictionary<Guid, Clientes> _customers = new();
        private readonly ConcurrentDictionary<string, Clientes> _customersByCpf = new();
        private readonly ConcurrentDictionary<string, Clientes> _customersByEmail = new();

        public Task<Clientes?> GetByIdAsync(Guid id)
        {
            _customers.TryGetValue(id, out var customer);
            return Task.FromResult(customer);
        }

        public Task<Clientes?> GetByCpfAsync(string cpf)
        {
            _customersByCpf.TryGetValue(cpf, out var customer);
            return Task.FromResult(customer);
        }

        public Task<Clientes?> GetByEmailAsync(string email)
        {
            _customersByEmail.TryGetValue(email, out var customer);
            return Task.FromResult(customer);
        }

        public Task AddAsync(Clientes customer)
        {
            if (customer.Id == Guid.Empty)
                customer.Id = Guid.NewGuid();

            _customers[customer.Id] = customer;
            _customersByCpf[customer.CPF] = customer;
            _customersByEmail[customer.Email] = customer;

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Clientes customer)
        {
            if (_customers.ContainsKey(customer.Id))
            {
                var existingCustomer = _customers[customer.Id];

                // Remover entradas antigas se CPF ou Email mudaram
                if (existingCustomer.CPF != customer.CPF)
                {
                    _customersByCpf.TryRemove(existingCustomer.CPF, out _);
                    _customersByCpf[customer.CPF] = customer;
                }

                if (existingCustomer.Email != customer.Email)
                {
                    _customersByEmail.TryRemove(existingCustomer.Email, out _);
                    _customersByEmail[customer.Email] = customer;
                }

                _customers[customer.Id] = customer;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            if (_customers.TryRemove(id, out var customer))
            {
                _customersByCpf.TryRemove(customer.CPF, out _);
                _customersByEmail.TryRemove(customer.Email, out _);
            }

            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            // Não há necessidade de salvar mudanças em memória
            return Task.CompletedTask;
        }

        public Task<List<Clientes>> GetAllAsync()
        {
            return Task.FromResult(_customers.Values.ToList());
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(_customers.Count);
        }
    }
}

