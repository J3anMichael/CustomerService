using System.Threading.Tasks;

namespace CustomerService.Infrastructure.Services.Interfaces
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(string queueName, T message) where T : class;
        Task PublishAsync<T>(string exchangeName, string routingKey, T message) where T : class;
    }
}