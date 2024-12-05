using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } = "User";
        public bool IsBlocked { get; set; } = false;

        public Cart Cart { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<WishList> WishLists { get; set; }
    }


}
