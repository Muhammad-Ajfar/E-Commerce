using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }

}
