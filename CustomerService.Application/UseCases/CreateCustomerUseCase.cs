using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Repositories;
using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Exceptions;
using System.Threading.Tasks;

namespace CustomerService.Application.UseCases
{
    public class CreateCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBusService _messageBusService;

        public CreateCustomerUseCase(ICustomerRepository customerRepository, IMessageBusService messageBusService)
        {
            _customerRepository = customerRepository;
            _messageBusService = messageBusService;
        }

        public async Task<CustomerResponse> ExecuteAsync(CreateCustomerCommand command)
        {
            // Verificar se já existe cliente com esse documento
            var existingCustomer = await _customerRepository.ExistsByDocumentAsync(command.Document);
            if (existingCustomer)
                throw new DomainException("Já existe um cliente com esse documento");

            // Criar novo cliente
            var customer = new Customer(command.Name, command.Email, command.Document, command.Phone);

            // Salvar no repositório
            await _customerRepository.CreateAsync(customer);

            // Atualizar status para indicar que proposta de crédito foi solicitada
            customer.UpdateStatus(CustomerStatus.CreditProposalRequested);
            await _customerRepository.UpdateAsync(customer);

            // Publicar evento para solicitar proposta de crédito
            var creditProposalRequest = new
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                CustomerEmail = customer.Email,
                CustomerDocument = customer.Document,
                CustomerPhone = customer.Phone,
                RequestedAt = System.DateTime.UtcNow
            };

            await _messageBusService.PublishAsync("credit-proposal-request", creditProposalRequest);

            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Document = customer.Document,
                Phone = customer.Phone,
                Status = customer.Status,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }
    }
}