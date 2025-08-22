using CustomerService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.API.Requests
{
    public class UpdateCustomerStatusRequest
    {
        [Required]
        public CustomerStatus Status { get; set; }
    }
}