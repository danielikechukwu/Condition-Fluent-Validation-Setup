using ConditionalFluentValidationSetup.Data;
using ConditionalFluentValidationSetup.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ConditionalFluentValidationSetup.Validators
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {

        public OrderDTOValidator(ECommerceDbContext context)
        {

            // ----- General Validations -----

            // Validate that CustomerId is provided and exists in the database
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .MustAsync(async (customerId, cancellation) =>
                {
                    return await context.Customers.AnyAsync(c => c.CustomerId == customerId, cancellation);
                }).WithMessage("CustomerId does not exist.");

            // Validate PaymentMode is provided and one of the allowed values
            RuleFor(x => x.PaymentMode)
                .NotEmpty().WithMessage("PaymentMode is required.")
                .Must(mode => new List<string> { "CreditCard", "UPI", "Cash" }.Contains(mode))
                .WithMessage("Invalid payment mode. Allowed values: CreditCard, UPI, Cash.");

            // Order amount must be greater than zero
            RuleFor(x => x.OrderAmount)
                .GreaterThan(0).WithMessage("OrderAmount must be greater than zero.");

            // Shipping address is required
            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Shipping Address is required.");

            // ----- Conditional Validations -----

            // If PaymentMode is UPI, then UPIId must be provided
            RuleFor(order => order.UPIId)
                .NotEmpty()
                .When(order => order.PaymentMode == "UPI")
                .WithMessage("UPIId is required for UPI payments.");

            // Another way to use When Method
            When(order => order.PaymentMode == "UPI", () =>
            {
                RuleFor(order => order.UPIId)
                .NotEmpty().WithMessage("UPIId is required for UPI payments.");
            });

            // If PaymentMode is CreditCard, then CreditCardNumber must be provided
            //RuleFor(order => order.CreditCardNumber)
            //    .NotEmpty()
            //    .When(order => order.PaymentMode == "CreditCard")
            //    .WithMessage("CreditCardNumber is required for Credit Card payments.");

            // Alternative
            // Another way : If PaymentMode is CreditCard, then CreditCardNumber must be provided
            When(order => order.PaymentMode == "CreditCard", () =>
            {
                RuleFor(order => order.CreditCardNumber)
                    .NotEmpty()
                    .WithMessage("CreditCardNumber is required for Credit Card payments.");
            });

            // For non-cash payments, ensure Discount is within an acceptable range (10% to 30%).
            // Using 'Unless' to skip this rule if PaymentMode is "Cash"
            //RuleFor(order => order.Discount)
            //    .InclusiveBetween(10, 30)
            //    .Unless(order => order.PaymentMode == "Cash")
            //    .WithMessage("Discount must be between 10% and 30% for non-cash payments.");

            // Alternative
            // Another way to use Unless Method
            Unless(order => order.PaymentMode == "Cash", () =>
            {
                RuleFor(order => order.Discount)
                    .InclusiveBetween(10, 30)
                    .WithMessage("Discount must be between 10% and 30% for non-cash payments.");
            });
        }
    }
}
