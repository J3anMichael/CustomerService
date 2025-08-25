using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Domain.Events
{
    public class StatusPropostaCreditoEvent : INotification
    {
        public Guid ClienteId { get; set; }
        public Guid PropostaId { get; set; }
        public string Status { get; set; } // APROVADA, REJEITADA, FALHA
        public decimal? LimiteAprovado { get; set; }
        public decimal? TaxaJuros { get; set; }
        public string Detalhes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
