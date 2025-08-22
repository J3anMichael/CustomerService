using System.ComponentModel.DataAnnotations;

namespace CustomerService.API.Requests
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Documento é obrigatório")]
        public string Document { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; set; }
    }
}