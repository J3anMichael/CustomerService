using CustomerService.Application.DTOs;
using CustomerService.Application.UseCases;
using CustomerService.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerService.Application.EventHandlers
{
    public class CreditProposalResponseEventHandler
    {
        private readonly UpdateCustomerStatusUseCase _updateCustomerStatusUseCase;
        private readonly ILogger<CreditProposalResponseEventHandler> _logger;

        public CreditProposalResponseEventHandler(
            UpdateCustomerStatusUseCase updateCustomerStatusUseCase,
            ILogger<CreditProposalResponseEventHandler> logger)
        {
            _updateCustomerStatusUseCase = updateCustomerStatusUseCase;
            _logger = logger;
        }

        public async Task HandleAsync(string message)
        {
            try
            {
                var creditProposalResponse = JsonSerializer.Deserialize<CreditProposalResponse>(message);

                var status = creditProposalResponse.Approved
                    ? CustomerStatus.CreditProposalApproved
                    : CustomerStatus.CreditProposalRejected;

                var command = new UpdateCustomerStatusCommand
                {
                    CustomerId = creditProposalResponse.CustomerId,
                    Status = status
                };

                await _updateCustomerStatusUseCase.ExecuteAsync(command);

                _logger.LogInformation("Credit proposal response processed for customer {CustomerId}. Approved: {Approved}",
                    creditProposalResponse.CustomerId, creditProposalResponse.Approved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing credit proposal response: {Message}", message);
                throw;
            }
        }

        private class CreditProposalResponse
        {
            public Guid CustomerId { get; set; }
            public bool Approved { get; set; }
            public string Reason { get; set; }
            public DateTime ProcessedAt { get; set; }
        }
    }
}