using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FalhaGeracaoCartao
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private readonly IChannel _canal;
        private readonly string _exchange = "cartao.exchange";
        private readonly string _fila = "cartao.falha.geracao.queue"; // nome da fila
        private readonly string _routingKey = "cartao.falha.geracao";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",  // ajuste via configuração (IOptions<RabbitMQSettings>) se precisar
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _canal = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            // Declara a exchange e a fila
            _canal.ExchangeDeclareAsync(exchange: _exchange, type: ExchangeType.Direct, durable: true);
            _canal.QueueDeclareAsync(queue: _fila, durable: true, exclusive: false, autoDelete: false);
            _canal.QueueBindAsync(queue: _fila, exchange: _exchange, routingKey: _routingKey);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new CustomAsyncConsumer(_canal, _logger);

            _canal.BasicConsumeAsync(
                queue: _fila,
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            if (_canal != null)
            {
                await _canal.CloseAsync();
                _canal.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            await base.StopAsync(cancellationToken);
        }
    }

    // Consumer assíncrono para RabbitMQ.Client 7.x
    public class CustomAsyncConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ILogger _logger;

        public CustomAsyncConsumer(IChannel channel, ILogger logger) : base(channel)
        {
            _logger = logger;
        }

        public override async Task HandleBasicDeliverAsync(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IReadOnlyBasicProperties properties,
            ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = default)
        {
            var mensagem = Encoding.UTF8.GetString(body.ToArray());

            try
            {
                var evento = JsonSerializer.Deserialize<CartaoEvent>(mensagem);

                _logger.LogWarning("?? Falha na geração do cartão - ClienteId: {ClienteId}, Status: {Status}, Erro: {MensagemErro}",
                    evento?.ClienteId, evento?.Status, evento?.MensagemErro);

                // --- Simulação de atualização de status ---
                // await _cartaoRepository.AtualizarStatusCartao(evento.ClienteId, "Falha");
                // await _eventBus.PublicarEvento(new ClienteAtualizacaoStatusEvent { ClienteId = evento.ClienteId, Status = "CartaoFalha" });

                // Confirma o processamento
                await Channel.BasicAckAsync(deliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Erro ao processar falha de cartão");
                await Channel.BasicNackAsync(deliveryTag, multiple: false, requeue: true);
            }
        }
    }

    // DTO do evento de falha
    public class CartaoEvent
    {
        public string ClienteId { get; set; }
        public string Status { get; set; }
        public string MensagemErro { get; set; }
    }
}
