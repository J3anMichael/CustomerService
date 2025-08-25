using CustomerService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Application.DTOs
{
    public record AtualizarStatusClienteCommand
    {
        public AtualizarStatusClienteCommand(Guid customerId, CadastroStatus status)
        {
            CustomerId = customerId;
            Status = status;
        }

        [Required]
        public Guid CustomerId { get; init; }

        [Required]
        public CadastroStatus Status { get; init; }
    }
}