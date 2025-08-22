using CustomerService.API.Requests;
using CustomerService.Application.DTOs;
using CustomerService.Application.UseCases;
using CustomerService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerUseCase _createCustomerUseCase;
        private readonly UpdateCustomerStatusUseCase _updateCustomerStatusUseCase;

        public CustomersController(
            CreateCustomerUseCase createCustomerUseCase,
            UpdateCustomerStatusUseCase updateCustomerStatusUseCase)
        {
            _createCustomerUseCase = createCustomerUseCase;
            _updateCustomerStatusUseCase = updateCustomerStatusUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                var command = new CreateCustomerCommand
                {
                    Name = request.Name,
                    Email = request.Email,
                    Document = request.Document,
                    Phone = request.Phone
                };

                var response = await _createCustomerUseCase.ExecuteAsync(command);
                return CreatedAtAction(nameof(GetCustomer), new { id = response.Id }, response);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<CustomerResponse>> UpdateCustomerStatus(
            Guid id,
            [FromBody] UpdateCustomerStatusRequest request)
        {
            try
            {
                var command = new UpdateCustomerStatusCommand
                {
                    CustomerId = id,
                    Status = request.Status
                };

                var response = await _updateCustomerStatusUseCase.ExecuteAsync(command);
                return Ok(response);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetCustomer(Guid id)
        {
            // Este endpoint pode ser implementado posteriormente com um GetCustomerUseCase
            return Ok(new { message = "Endpoint para consulta será implementado" });
        }
    }
}