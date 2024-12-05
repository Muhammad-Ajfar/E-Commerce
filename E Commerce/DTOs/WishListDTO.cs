using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTOs
{
    public class WishListDTO
    {

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}
