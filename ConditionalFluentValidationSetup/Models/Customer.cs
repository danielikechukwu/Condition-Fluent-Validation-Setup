namespace ConditionalFluentValidationSetup.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        // Customer's full name
        public string Name { get; set; }
        // Navigation property for related orders
        public ICollection<Order> Orders { get; set; }
    }
}
