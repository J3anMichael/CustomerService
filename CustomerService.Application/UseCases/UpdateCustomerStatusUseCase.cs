using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Repositories;
using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Exceptions;
using System.Threading.Tasks;

namespace CustomerService.Application.UseCases
{
    public class UpdateCustomerStatusUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBusService _messageBusService;

        public UpdateCustomerStatusUseCase(ICustomerRepository customerRepository, IMessageBusService messageBusService)
        {
            _customerRepository = customerRepository;
            _messageBusService = messageBusService;
        }

        public async Task<CustomerResponse> ExecuteAsync(UpdateCustomerStatusCommand command)
        {
            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer == null)
                throw new DomainException("Cliente não encontrado");

            var previousStatus = customer.Status;
            customer.UpdateStatus(command.Status);
            await _customerRepository.UpdateAsync(customer);

            // Se a proposta de crédito foi aprovada, solicitar emissão de cartão
            if (command.Status == CustomerStatus.CreditProposalApproved &&
                previousStatus == CustomerStatus.CreditProposalRequested)
            {
                customer.UpdateStatus(CustomerStatus.CardRequested);
                await _customerRepository.UpdateAsync(customer);

                var cardRequest = new
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    CustomerDocument = customer.Document,
                    RequestedAt = System.DateTime.UtcNow
                };

                await _messageBusService.PublishAsync("card-request", cardRequest);
            }

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