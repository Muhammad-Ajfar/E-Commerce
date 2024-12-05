namespace E_Commerce.DTOs
{
    public class WishListViewDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
