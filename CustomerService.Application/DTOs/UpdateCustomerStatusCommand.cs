using CustomerService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Application.DTOs
{
    public record UpdateCustomerStatusCommand
    {
        [Required]
        public Guid CustomerId { get; init; }

        [Required]
        public CustomerStatus Status { get; init; }
    }
}