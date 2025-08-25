using Cliente.Domain.Events;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustomerService.Domain.Events.ClienteCadastradoEvent;

namespace CustomerService.Application.Commands.Handlers
{
    public class StatusCreditoPropostaEventHandler : INotificationHandler<StatusPropostaCreditoEvent>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILogger<StatusCreditoPropostaEventHandler> _logger;

        public StatusCreditoPropostaEventHandler(
            IClienteRepository clienteRepository,
            IMessageBus messageBus,
            ILogger<StatusCreditoPropostaEventHandler> logger)
        {
            _clienteRepository = clienteRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        public async Task Handle(StatusPropostaCreditoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processando evento de proposta: {CustomerId} - {Status}",
                notification.ClienteId, notification.Status);

            var customer = await _clienteRepository.GetByIdAsync(notification.ClienteId);
            if (customer == null)
            {
                _logger.LogWarning("Cliente não encontrado: {CustomerId}", notification.ClienteId);
                return;
            }

            switch (notification.Status.ToUpper())
            {
                case "APROVADO":
                    customer.UpdateStatus(CadastroStatus.PropostaCreditoAprovada,
                        $"Proposta aprovada. Limite: R$ {notification.LimiteAprovado:N2}");
                    _logger.LogInformation("Proposta aprovada para cliente {CustomerId}", customer.Id);
                    break;

                case "REPROVADO":
                    customer.UpdateStatus(CadastroStatus.PropostaCreditoRejeitada, notification.Detalhes);
                    _logger.LogInformation("Proposta rejeitada para cliente {CustomerId}", customer.Id);
                    break;

                case "FALHA":
                    customer.UpdateStatus(CadastroStatus.Falha, notification.Detalhes);
                    _logger.LogWarning("Falha na proposta para cliente {CustomerId}: {Details}",
                        customer.Id, notification.Detalhes);

                    // Agendar retry
                    await _messageBus.PublishAsync("customer.process.retry", new EventoTentativaProcessamentoCliente
                    {
                        ClienteId = customer.Id,
                        TipoProcesso = "PROPOSAL",
                        Motivo = notification.Detalhes,
                        ContadorTentativas = 1,
                        Timestamp = DateTime.UtcNow
                    });
                    break;
            }

            await _clienteRepository.UpdateAsync(customer);
            await _clienteRepository.SaveChangesAsync();
        }
    }
}
