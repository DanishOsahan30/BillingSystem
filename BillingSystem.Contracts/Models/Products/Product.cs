using System.ComponentModel.DataAnnotations;

namespace BillingSystem.Models.Products
{
    public class Product
    {
     
        public int ID { get; set; }

        [Required(ErrorMessage ="ProductName is Required")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price is invalid")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "SubCategory is required")]
        public string SubCategory { get; set; }
    }
}
