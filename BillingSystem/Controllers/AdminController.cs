using BiilingSystem.Services.Services.AgentServices;
using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Models.Authentication;
using BillingSystem.Models.Products;
using BillingSystem.Services.AdminServices;
using BillingSystem.Services.HelperServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        private readonly IHelperService _helperService;
        private readonly IAgentService _agentService;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,IAdminService adminService,IHelperService helperService,IAgentService agentService, ILogger<AdminController> logger)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _adminService = adminService;
            _helperService = helperService;
            _agentService = agentService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var invoices=await _adminService.FetchInvoices();
                return View(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);

                throw;
            }
          
        }

        public async Task<IActionResult> Agent()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        public IActionResult CreateAgent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAgent(RegisterUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Name=_helperService.CapitalizedString(user.Name);
                    var checkNameExsist = await _userManager.FindByNameAsync(user.Name);
                    if (checkNameExsist != null)
                    {
                        ModelState.AddModelError("Name", "Username Already Exsists . Choose Another one");
                        return View(user);
                    }
                    var checkEmailExsist = await _userManager.FindByEmailAsync(user.Email);
                    if (checkEmailExsist != null)
                    {
                        ModelState.AddModelError("Email", "Email Already Exsists . Choose Another one");
                        return View(user);
                    }
                    bool result=await _adminService.AddAccount(user);
                    if(result)
                    {

                        TempData["success"] = "User Created Successfully";
                        return RedirectToAction("Agent");
                    }
                    else
                    {
                        TempData["error"] = "Failed to create user account.";
                        return RedirectToAction("Agent");
                    }
                }
                return RedirectToAction("Agent");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
           
        }


        [HttpGet]
        public async Task<IActionResult> DeleteAgent(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User Id cannot be null or empty");
                }
                 
                bool result= await _adminService.RemoveAccount(userId);
                if(result)
                {
                    TempData["success"] = "Agent deleted successfully";
                    return RedirectToAction("Agent");
                }
                else
                {

                    TempData["error"] = "Failed to create user account.";
                    return RedirectToAction("Agent");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }

       
        public async Task<IActionResult> Products()
        {
            try
            {
                var productList = await _adminService.FetchProducts();
                ViewBag.ProductList = productList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    category.Name=_helperService.CapitalizedString(category.Name);
                    bool categorExsist = await _adminService.CategoryExists(category.Name);
                    if(categorExsist)
                    {
                        TempData["error"] = "Category Already Exsists";
                        return RedirectToAction("Products");
                    }
                    bool result=await _adminService.AddCategory(category);
                    if(result)
                    {
                        TempData["success"] = "Category Added Successfully";
                        return RedirectToAction("Products");
                    }
                    else
                    {
                        TempData["error"] = "An error occurred while processing your request.";
                        return RedirectToAction("Products");
                    }
                }
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubCategory(SubCategory subCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subCategory.Name = _helperService.CapitalizedString(subCategory.Name);
                    bool categorExsist = await _adminService.SubCategoryExists(subCategory.Name);
                    if (categorExsist)
                    {
                        TempData["error"] = "SubCategory Already Exsists";
                        return RedirectToAction("Products");
                    }
                    bool result = await _adminService.AddSubCategory(subCategory);
                    if (result)
                    {
                        TempData["success"] = "SubCategory Added Successfully";
                        return RedirectToAction("Products");
                    }
                    else
                    {
                        TempData["error"] = "An error occurred while processing your request.";
                        return RedirectToAction("Products");
                    }
                }
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }

        public async Task <IActionResult> AddProduct()
        {
            try
            {
                var categories =await  _adminService.FetchCategories();
                var subCategories=await _adminService.FetchSubCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                ViewBag.SubCategories = new SelectList(subCategories, "Name", "Name");
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    product.Name = _helperService.CapitalizedString(product.Name);
                    bool checkProductExsist = await _adminService.CheckProductExsist(product.Name);
                    if (checkProductExsist)
                    {
                        TempData["error"] = "Product Already Exsists";
                        return RedirectToAction("Products");
                    }
                    bool result = await _adminService.AddProduct(product);
                    if (result)
                    {
                        TempData["success"] = "Product Added Successfully";
                        return RedirectToAction("Products");
                    }
                    else
                    {
                        TempData["error"] = "Some error occured while adding the product .";
                        return RedirectToAction("Products");
                    }
                }
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var product = await _adminService.FindProduct(productId);
                var result = await _adminService.RemoveProduct(product);
                if (result)
                {
                    TempData["success"] = "Product Deleted Successfully";
                    return RedirectToAction("Products");
                }
                else
                {
                    TempData["error"] = "Some error occurred while deleting the product";
                    return RedirectToAction("Products");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }

        public async Task<IActionResult> UpdateProduct(int productId)
        {
            try
            {
                var product = await _adminService.FindProduct(productId);
                var categories = await _adminService.FetchCategories();
                var subCategories = await _adminService.FetchSubCategories();

                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                ViewBag.SubCategories = new SelectList(subCategories, "Name", "Name");

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Products");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool result = await _adminService.UpdateProduct(product);
                    if (result)
                    {
                        TempData["success"] = "Product updated Successfully";
                        return RedirectToAction("Products");
                    }
                    else
                    {
                        TempData["error"] = "Some error occured while updating the Product";
                        return RedirectToAction("Products");
                    }
                }
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Products");
            }
        }


        public IActionResult ShowInfo(int invoicenumber)
        {
            try
            {
                IEnumerable<UserInvoiceModel> products = _agentService.GetProductsByInvoiceNumber(invoicenumber);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "Some error occured while fetching info";
                return RedirectToAction("Index");
                throw;
            }
        }

    }
}
