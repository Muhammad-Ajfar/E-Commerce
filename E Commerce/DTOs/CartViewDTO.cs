namespace E_Commerce.DTOs
{
    public class CartViewDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
