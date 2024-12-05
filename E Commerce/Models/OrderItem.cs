using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero.")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

}
