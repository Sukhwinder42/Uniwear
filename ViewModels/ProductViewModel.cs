using System.ComponentModel.DataAnnotations;
using Uniwear.Models;

namespace Uniwear.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public List<Category>? Categories { get; set; }

        public string GenderGroup { get; set; }
    }
}
