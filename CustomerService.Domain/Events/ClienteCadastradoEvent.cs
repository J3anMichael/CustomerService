using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Domain.Events
{
    public class ClienteCadastradoEvent : INotification
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
        public DateTime DataCriacao { get; set; }

        // Propriedades do evento
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string TipoEvento { get; set; } = "ClienteCriado";
        public int Versao { get; set; } = 1;
    }
}
