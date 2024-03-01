using System.ComponentModel.DataAnnotations;

namespace BillingSystem.Models.Products
{
    public class SubCategory
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "SubCategoryName is required")]
        [Display(Name = "SubCategory")]
        public string Name { get; set; }
    }
}
