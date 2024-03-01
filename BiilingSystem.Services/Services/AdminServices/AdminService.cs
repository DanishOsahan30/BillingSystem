using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Data;
using BillingSystem.Models.Authentication;
using BillingSystem.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        UserManager<IdentityUser> _userManager;
        public AdminService(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Adding Services
        public async Task<bool> AddAccount(RegisterUser user)
        {

            IdentityUser agentUser = new() { Email = user.Email, UserName = user.Name, PhoneNumber = user.PhoneNumber };
            var result = await _userManager.CreateAsync(agentUser, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(agentUser, user.Role);
                return true;
            }
            return false;
        }
        public async Task<bool> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            int entitiesWritten = await _context.SaveChangesAsync();
            return entitiesWritten > 0;
        }
        public async Task<bool> AddSubCategory(SubCategory subcategory)
        {
            _context.SubCategories.Add(subcategory);
            int entitiesWritten = await _context.SaveChangesAsync();
            return entitiesWritten > 0;
        }
        public async Task<bool> AddProduct(Product product)
        {
            _context.Products.Add(product);
            int entitiesWritten = await _context.SaveChangesAsync();
            return entitiesWritten > 0;
        }

        //Removal Services
        public async Task<bool> RemoveAccount(string userId)
        {
            var agent = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.DeleteAsync(agent);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            int entitiesAffected = await _context.SaveChangesAsync();
            return entitiesAffected > 0;
        }

        //Checking Services
        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }
        public async Task<bool> SubCategoryExists(string subCategoryName)
        {
            return await _context.SubCategories.AnyAsync(c => c.Name == subCategoryName);
        }
        public async Task<bool> CheckProductExsist(string productName)
        {
            return await _context.Products.AnyAsync(c => c.Name == productName);
        }



        //Fetching Services
        public async Task<IEnumerable<Category>> FetchCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
        public async Task<IEnumerable<SubCategory>> FetchSubCategories()
        {
            var Subcategories = await _context.SubCategories.ToListAsync();
            return Subcategories;
        }
        public async Task<List<Product>> FetchProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }
        public async Task<Product> FindProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product;
        }
        public async Task<IEnumerable<Invoice>> FetchInvoices()
        {
            var invoices = await _context.Invoices.ToListAsync();
            return invoices;
        }


        //Update Services
        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            int entitiesAffected = await _context.SaveChangesAsync();
            return entitiesAffected > 0;
        }





    }

}
