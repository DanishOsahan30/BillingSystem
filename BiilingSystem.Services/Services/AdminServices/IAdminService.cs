using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Models.Authentication;
using BillingSystem.Models.Products;

namespace BillingSystem.Services.AdminServices
{
    public interface IAdminService
    {
        //Add services
        Task<bool> AddAccount(RegisterUser user);
        Task<bool> AddCategory(Category category);
        Task<bool> AddSubCategory(SubCategory subcategory);
        Task<bool> AddProduct(Product product);

        //Checking  services
        Task<bool> CategoryExists(string categoryName);
        Task<bool> SubCategoryExists(string subCategoryName);
        Task<bool> CheckProductExsist(string productName);

        //Fetch services
        Task<IEnumerable<Category>> FetchCategories();
        Task<IEnumerable<SubCategory>> FetchSubCategories();
        Task<List<Product>> FetchProducts();
        Task<Product> FindProduct(int productId);
        Task<IEnumerable<Invoice>> FetchInvoices();


        //Deleting Services
        Task<bool> RemoveAccount(string userId);
        Task<bool> RemoveProduct(Product product);

        //Updating Services
        Task<bool> UpdateProduct(Product product);



    }
}
