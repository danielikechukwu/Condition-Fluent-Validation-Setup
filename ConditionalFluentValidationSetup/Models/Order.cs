using System.ComponentModel.DataAnnotations.Schema;

namespace ConditionalFluentValidationSetup.Models
{
    public class Order
    {
        public int OrderId { get; set; } // Primary key

        // Foreign key linking to the Customer entity
        public int CustomerId { get; set; }

        // Navigation property for the associated Customer
        public Customer Customer { get; set; }

        // Payment mode: "CreditCard", "UPI", or "Cash"
        public string PaymentMode { get; set; }

        // Credit card number if PaymentMode is CreditCard
        public string? CreditCardNumber { get; set; }

        // UPI ID if PaymentMode is UPI
        public string? UPIId { get; set; }

        // Total amount for the order
        [Column(TypeName = "decimal(8,2)")]
        public decimal OrderAmount { get; set; }

        // Discount percentage applied to the order
        [Column(TypeName = "decimal(8,2)")]
        public decimal Discount { get; set; }

        // The date when the order is placed
        public DateTime OrderDate { get; set; }

        // Shipping address for the order
        public string ShippingAddress { get; set; }
    }
}
