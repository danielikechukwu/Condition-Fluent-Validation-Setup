using ConditionalFluentValidationSetup.Data;
using ConditionalFluentValidationSetup.DTOs;
using ConditionalFluentValidationSetup.Models;
using ConditionalFluentValidationSetup.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConditionalFluentValidationSetup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrdersController(ECommerceDbContext context)
        {
            _context = context;
        }

        // Creates a new order based on the provided OrderDTO.
        // Applies conditional validations and returns errors if any.
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            // Initialize the validator with the current DbContext for any database-related checks.
            var validator = new OrderDTOValidator(_context);

            // Validate the incoming OrderDTO.
            var validationResult = await validator.ValidateAsync(orderDTO);

            // If validation fails, return a 400 Bad Request with the details.
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors
                    .Select(e => new 
                    { 
                      Field = e.PropertyName, // Property that failed validation
                      Error = e.ErrorMessage // Detailed error message
                    })
                    .ToList();

                return BadRequest(new { Errors = errorResponse });
            }

            // Map the validated DTO to the Order entity.'
            var order = new Order
            {
                CustomerId = orderDTO.CustomerId,
                PaymentMode = orderDTO.PaymentMode,
                OrderAmount = orderDTO.OrderAmount,
                ShippingAddress = orderDTO.ShippingAddress,
                UPIId = orderDTO.UPIId,
                CreditCardNumber = orderDTO.CreditCardNumber,
                OrderDate = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Order created successfully.",
                OrderId = order.OrderId
            });
        }
    }
}
