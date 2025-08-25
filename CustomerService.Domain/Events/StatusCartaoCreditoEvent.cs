using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Domain.Events
{
    public class StatusCartaoCreditoEvent : INotification
    {
        public Guid ClienteId { get; set; }
        public Guid CartaoId { get; set; }
        public Guid PropostaId { get; set; }
        public string Status { get; set; } // EMITIDO, FALHA
        public string NumeroCartao { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal? LimiteCredito { get; set; }
        public string Detalhes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
