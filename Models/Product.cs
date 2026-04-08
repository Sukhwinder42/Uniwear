using System.ComponentModel.DataAnnotations.Schema;

namespace Uniwear.Models
{
    public class Product
    {
        public int Id { get; set; }   // Primary Key

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }   // Foreign Key

        public string? GenderGroup { get; set; }  //for ladies/men/kids 

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public Category? Category { get; set; } // Navigation property
        public ICollection<OrderItem> OrderItem { get; set; } // Navigation property
    }
}
