using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Domain.Events
{
    public class EventoTentativaProcessamentoCliente : INotification
    {
        public Guid ClienteId { get; set; }
        public string TipoProcesso { get; set; } // PROPOSTA, CARTAO
        public string Motivo { get; set; }
        public int ContadorTentativas { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
