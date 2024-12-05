using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTOs
{
    public class ProductDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }


        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number.")]
        public int Stock { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "MRP must be greater than zero.")]
        public decimal MRP { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(50, ErrorMessage = "Category Name must not exceed 50 characters.")]
        public string CategoryName { get; set; }

    }
}
