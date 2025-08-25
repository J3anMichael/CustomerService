using Cliente.API.Requests;
using CustomerService.Application.Commands;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Cliente.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastroClienteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CadastroClienteController> _logger;

        public CadastroClienteController(IMediator mediator, ILogger<CadastroClienteController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        /// <param name="request">Dados do cliente a ser cadastrado</param>
        /// <returns>Dados do cliente cadastrado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CadastroResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CadastrarCliente([FromBody] CadastroClienteRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro de cliente: {CPF}", request.CPF);

                var command = new CadastroClienteCommand(
                    request.Nome,
                    request.Email,
                    request.CPF,
                    request.Telefone,
                    request.DataNascimento,
                    request.Endereco,
                    request.Cidade,
                    request.CEP,
                    request.Estado
                );

                var response = await _mediator.Send(command);

                _logger.LogInformation("Cliente cadastrado com sucesso. ID: {CustomerId}", response.Id);

                return CreatedAtAction(
                    nameof(ObterCliente),
                    new { id = response.Id },
                    response);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Erro de validação ao cadastrar cliente: {Message}", ex.Message);
                return BadRequest(new
                {
                    message = "Dados inválidos",
                    errors = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflito ao cadastrar cliente: {Message}", ex.Message);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao cadastrar cliente");
                return StatusCode(500, new
                {
                    message = "Erro interno do servidor"
                });
            }
        }

        /// <summary>
        /// Busca um cliente por ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Dados do cliente</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterCliente(Guid id)
        {
            return Ok(new { message = "Endpoint para buscar cliente por ID" });
        }
    }
}