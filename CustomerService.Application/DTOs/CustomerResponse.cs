using CustomerService.Domain.Enums;
using System;

namespace CustomerService.Application.DTOs
{
    public record CustomerResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Document { get; init; }
        public string Phone { get; init; }
        public CustomerStatus Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}