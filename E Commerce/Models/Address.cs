using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Address
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

}
