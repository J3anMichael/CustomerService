using CustomerService.Domain.Enums;
using CustomerService.Domain.Exceptions;

namespace CustomerService.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }
        public string Phone { get; private set; }
        public CustomerStatus Status { get; private set; }

        private Customer() { } // Para EF Core

        public Customer(string name, string email, string document, string phone)
        {
            ValidateData(name, email, document, phone);

            Name = name;
            Email = email;
            Document = document;
            Phone = phone;
            Status = CustomerStatus.Pending;
        }

        public void UpdateStatus(CustomerStatus status)
        {
            Status = status;
            SetUpdatedAt();
        }

        private static void ValidateData(string name, string email, string document, string phone)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email é obrigatório");

            if (string.IsNullOrWhiteSpace(document))
                throw new DomainException("Documento é obrigatório");

            if (string.IsNullOrWhiteSpace(phone))
                throw new DomainException("Telefone é obrigatório");
        }
    }
}