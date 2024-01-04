using System.ComponentModel.DataAnnotations;

namespace aspsitekurs2.Models
{
    public class ProductEditModel
    {
        public int ID { get; set; }

        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Invalid name format")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Count must be greater than 0")]
        public int Count { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
        public IFormFile? NewPic { get; set; }
        public string? Pic { get; set; }
    }
}
