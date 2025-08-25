using CustomerService.Infrastructure.Configurations;
using CustomerService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerService.Infrastructure.Services
{
    public class MessageBus : IMessageBus, IDisposable
    {
        private readonly IConnection _connection;      
        private readonly IChannel _channel;
        private readonly RabbitMQConfiguration _config;
        private readonly ILogger<MessageBus> _logger;

        public MessageBus(IOptions<RabbitMQConfiguration> config, ILogger<MessageBus> logger)
        {
            _config = config.Value;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            SetupExchangeAndQueues();
        }

        private async void SetupExchangeAndQueues()
        {
            try
            {
                // Tentar verificar se a exchange existe passivamente
                try
                {
                    await _channel.ExchangeDeclarePassiveAsync(_config.ExchangeName);
                    _logger.LogInformation("Exchange já existe: {Exchange}", _config.ExchangeName);
                }
                catch
                {
                    // Se falhar, criar a exchange
                    _channel.ExchangeDeclareAsync(
                        exchange: "customer.exchange",
                        type: ExchangeType.Direct, // ou Fanout, Topic... depende do seu caso
                        durable: true,
                        autoDelete: false
                    );

                    _logger.LogInformation("Exchange criada: {Exchange}", _config.ExchangeName);
                }

                // Configurar filas e bindings
                await SetupQueueWithRetry("cliente.cadastrado.queue", "cliente.cadastrado.queue");
                //await SetupQueueWithRetry("cliente.processamento.queue", "cliente.processamento.queue");

                _logger.LogInformation("RabbitMQ Exchange e Filas configuradas com sucesso. Exchange: {Exchange}", _config.ExchangeName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao configurar exchange e filas");
                throw;
            }
        }

        private async Task SetupQueueWithRetry(string queueName, string routingKey)
        {
            try
            {
                // Tentar verificar se a queue existe passivamente
                try
                {
                    await _channel.QueueDeclarePassiveAsync(queueName);
                    _logger.LogInformation("Queue já existe: {Queue}", queueName);
                }
                catch
                {
                    // Se falhar, criar a queue
                    await _channel.QueueDeclareAsync(
                        queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    _logger.LogInformation("Queue criada: {Queue}", queueName);
                }

                // Fazer o binding (isso é idempotente - pode ser chamado múltiplas vezes)
                await _channel.QueueBindAsync(
                    queue: queueName,
                    exchange: _config.ExchangeName,
                    routingKey: routingKey);

                _logger.LogInformation("Binding criado: {Queue} -> {RoutingKey}", queueName, routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao configurar queue: {Queue}", queueName);
                throw;
            }
        }

        public async Task PublishAsync<T>(string routingKey, T message) where T : class
            => await PublishAsync(_config.ExchangeName, routingKey, message);

        public async Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class
        {
            try
            {
                var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var body = Encoding.UTF8.GetBytes(json);

                var properties = new BasicProperties
                {
                    Persistent = true,
                    MessageId = Guid.NewGuid().ToString(),
                    Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                    ContentType = "application/json",
                    DeliveryMode = (DeliveryModes)2
                };

                await _channel.BasicPublishAsync(
                    exchange: exchange,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation(
                    "Mensagem publicada com sucesso. Exchange: {Exchange}, RoutingKey: {RoutingKey}, MessageId: {MessageId}",
                    exchange, routingKey, properties.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem. Exchange: {Exchange}, RoutingKey: {RoutingKey}", exchange, routingKey);
                throw;
            }
        }

        public async void Dispose()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
    }
}