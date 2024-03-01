using System.ComponentModel.DataAnnotations;

namespace BillingSystem.Models.Products
{
    public class Category
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="CategoryName is required")]
        [Display(Name="Category")]
        public string Name { get; set; }
    }
}
