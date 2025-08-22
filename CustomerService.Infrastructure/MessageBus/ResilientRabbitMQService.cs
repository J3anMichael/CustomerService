using CustomerService.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading.Tasks;

namespace CustomerService.Infrastructure.MessageBus
{
    public class ResilientRabbitMQService : IMessageBusService
    {
        private readonly RabbitMQService _rabbitMQService;
        private readonly ILogger<ResilientRabbitMQService> _logger;
        private readonly IAsyncPolicy _retryPolicy;

        public ResilientRabbitMQService(RabbitMQService rabbitMQService, ILogger<ResilientRabbitMQService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning("Retry attempt {RetryCount} for message publishing. Waiting {Delay}ms",
                            retryCount, timespan.TotalMilliseconds);
                    });
        }

        public async Task PublishAsync<T>(string queueName, T message) where T : class
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _rabbitMQService.PublishAsync(queueName, message);
            });
        }

        public async Task PublishAsync<T>(string exchangeName, string routingKey, T message) where T : class
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _rabbitMQService.PublishAsync(exchangeName, routingKey, message);
            });
        }
    }
}