using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image {  get; set; }

        public int Stock { get; set; }

        public decimal MRP { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}
