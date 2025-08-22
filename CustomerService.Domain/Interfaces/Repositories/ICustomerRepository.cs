using CustomerService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CustomerService.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> GetByDocumentAsync(string document);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> ExistsByDocumentAsync(string document);
    }
}