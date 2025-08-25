using CustomerService.Domain.Enums;
using CustomerService.Domain.Exceptions;

namespace CustomerService.Domain.Entities
{
    public class Clientes : EntityBase
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public CadastroStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Ativo { get; set; }
        public List<ProcessoEvent> Events { get; set; }

        public Clientes()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Status = CadastroStatus.Pendente;
            Ativo = true;
            Events = new List<ProcessoEvent>();
        }

        public void UpdateStatus(CadastroStatus novoStatus, string details = null)
        {
            var statusAntigo = Status;
            Status = novoStatus;
            UpdatedAt = DateTime.UtcNow;

            Events.Add(new ProcessoEvent
            {
                Id = Guid.NewGuid(),
                ClienteId = Id,
                EventType = "STATUS_CHANGE",
                StatusAntigo = statusAntigo.ToString(),
                NovoStatus = novoStatus.ToString(),
                Detalhes = details,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public class ProcessoEvent
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string EventType { get; set; }
        public string StatusAntigo { get; set; }
        public string NovoStatus { get; set; }
        public string Detalhes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}