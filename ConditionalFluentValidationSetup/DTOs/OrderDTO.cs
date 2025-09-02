namespace ConditionalFluentValidationSetup.DTOs
{
    // DTO for creating a new order. Encapsulates order data from the client.
    public class OrderDTO
    {
        public int CustomerId { get; set; }

        // Payment mode should be "CreditCard", "UPI", or "Cash"
        public string PaymentMode { get; set; }

        // Required if PaymentMode is "CreditCard"
        public string? CreditCardNumber { get; set; }

        // Required if PaymentMode is "UPI"
        public string? UPIId { get; set; }

        // The total order amount; must be greater than zero
        public decimal OrderAmount { get; set; }

        // Discount percentage; validated conditionally
        public decimal Discount { get; set; }

        // Shipping address is required for order processing
        public string ShippingAddress { get; set; }
    }
}
