namespace E_Commerce.DTOs
{
    public class OrderGetDTO
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public ICollection<OrderItemGetDTO> OrderItems { get; set; }
    }
}
