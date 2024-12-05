using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTOs
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(50, ErrorMessage = "Category Name must not exceed 50 characters.")]
        public string Name { get; set; }
    }

}
