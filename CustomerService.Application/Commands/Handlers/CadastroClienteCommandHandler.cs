using CustomerService.Application.Commands;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Events;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Commanding;
using Polly;

namespace CustomerService.Application.Commands.Handlers
{
    public class CadastroClienteCommandHandler : IRequestHandler<CadastroClienteCommand, CadastroResponse>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILogger<CadastroClienteCommandHandler> _logger;
        private readonly IAsyncPolicy _retryPolicy;

        public CadastroClienteCommandHandler(
            IClienteRepository clienteRepository,
            IMessageBus messageBus,
            ILogger<CadastroClienteCommandHandler> logger)
        {
            _clienteRepository = clienteRepository;
            _messageBus = messageBus;
            _logger = logger;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning("Tentativa {RetryCount} de publicação falhou. Próxima em {Delay}s",
                            retryCount, timespan.TotalSeconds);
                    });
        }

        public async Task<CadastroResponse> Handle(CadastroClienteCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando processamento de cadastro de cliente: {Email}", command.Email);

            // Verificar duplicatas
            var existingByCpf = await _clienteRepository.GetByCpfAsync(command.CPF);
            if (existingByCpf != null)
                throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF.");

            var existingByEmail = await _clienteRepository.GetByEmailAsync(command.Email);
            if (existingByEmail != null)
                throw new InvalidOperationException("Já existe um cliente cadastrado com este email.");

            // Criar cliente
            var cliente = new Clientes
            {
                Nome = command.Nome,
                Email = command.Email,
                CPF = command.CPF,
                Telefone = command.Telefone,
                DataNascimento = command.DataNascimento,
                Endereco = command.Endereco,
                Cidade = command.Cidade,
                CEP = command.CEP,
                Estado = command.Estado
            };

            // Salvar no banco - In Memory
            await _clienteRepository.AddAsync(cliente);
            await _clienteRepository.SaveChangesAsync();

            _logger.LogInformation("Cliente salvo com sucesso: {CustomerId}", cliente.Id);

            // Criar evento
            var clienteCadastradoEvent = new ClienteCadastradoEvent
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone,
                DataNascimento = cliente.DataNascimento,
                Endereco = cliente.Endereco,
                Cidade = cliente.Cidade,
                CEP = cliente.CEP,
                Estado = cliente.Estado,
                DataCriacao = cliente.CreatedAt,
                EventId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow
            };

            // Publicar evento com retry policy
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _messageBus.PublishAsync("cliente.cadastrado.queue", clienteCadastradoEvent);
            });

            // Atualizar status
            cliente.UpdateStatus(CadastroStatus.Pendente, "Aguardando processamento da proposta de crédito");
            await _clienteRepository.UpdateAsync(cliente);
            await _clienteRepository.SaveChangesAsync();

            _logger.LogInformation("Evento CustomerCreated publicado com sucesso: {CustomerId}", cliente.Id);

            return new CadastroResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                CPF = cliente   .CPF,
                Status =    cliente.Status.ToString(),
                CreatedAt = cliente.CreatedAt,
                Mensagem = "Cliente cadastrado com sucesso! Proposta de crédito será processada em breve."
            };
        }
    }
}

