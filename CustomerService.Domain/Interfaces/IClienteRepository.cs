using CustomerService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CustomerService.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Clientes?> GetByIdAsync(Guid id);
        Task<Clientes?> GetByCpfAsync(string cpf);
        Task<Clientes?> GetByEmailAsync(string email);
        Task AddAsync(Clientes customer);
        Task UpdateAsync(Clientes customer);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync(); // Pode ser mantido para compatibilidade
        Task<List<Clientes>> GetAllAsync();
        Task<int> CountAsync();
    }
}