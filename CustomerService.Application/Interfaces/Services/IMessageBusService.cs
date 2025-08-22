using System.Threading.Tasks;

namespace CustomerService.Application.Interfaces.Services
{
    public interface IMessageBusService
    {
        Task PublishAsync<T>(string queueName, T message) where T : class;
        Task PublishAsync<T>(string exchangeName, string routingKey, T message) where T : class;
    }
}