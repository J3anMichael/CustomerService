using CustomerService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CustomerService.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Clientes> CreateAsync(Clientes customer);
        Task<Clientes> GetByIdAsync(Guid id);
        Task<Clientes> GetByDocumentAsync(string document);
        Task<Clientes> UpdateAsync(Clientes customer);
        Task<bool> ExistsByDocumentAsync(string document);
    }
}