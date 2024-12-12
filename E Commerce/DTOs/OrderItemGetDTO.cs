namespace E_Commerce.DTOs
{
    public class OrderItemGetDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
