using System.ComponentModel.DataAnnotations;

namespace CustomerService.Application.DTOs
{
    public record CreateCustomerCommand
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Documento é obrigatório")]
        public string Document { get; init; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; init; }
    }
}