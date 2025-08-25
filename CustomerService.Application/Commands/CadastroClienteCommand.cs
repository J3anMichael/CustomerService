using CustomerService.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Application.Commands
{
    public class CadastroClienteCommand : IRequest<CadastroResponse>
    {
        public CadastroClienteCommand(string nome, string email, string cpf, string telefone,
                                    DateTime dataNascimento, string endereco, string cidade,
                                    string cep, string estado)
        {
            Nome = nome;
            Email = email;
            CPF = cpf;
            Telefone = telefone;
            DataNascimento = dataNascimento;
            Endereco = endereco;
            Cidade = cidade;
            CEP = cep;
            Estado = estado;
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }


    }
}