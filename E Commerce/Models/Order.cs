using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
