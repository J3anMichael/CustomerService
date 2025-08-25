using CustomerService.Domain.Enums;
using System;

namespace CustomerService.Application.DTOs
{
    public record CadastroResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Mensagem { get; set; }
    }
    public class CustomerStatusResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ProcessoEventDto> Events { get; set; }
    }

    public class ProcessoEventDto
    {
        public string EventType { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}