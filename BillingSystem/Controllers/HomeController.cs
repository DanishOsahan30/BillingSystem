using BiilingSystem.Services.Services.AgentServices;
using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Models;
using BillingSystem.Models.Products;
using BillingSystem.Services.HelperServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace BillingSystem.Controllers
{
    [Authorize(Roles = "Agent")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAgentService _agentService;
        private readonly IHelperService _helperService;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IAgentService agentService, IHelperService helperService)
        {
            _userManager = userManager;
            _agentService = agentService;
            _helperService = helperService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("First log created $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            var users = await _userManager.Users.ToListAsync();
            var customers = users.Where(u => _userManager.IsInRoleAsync(u, "Customer").Result).ToList();
            ViewBag.Customers = new SelectList(customers, "UserName", "UserName");
            var produtcs = await _agentService.GetProducts();
            ViewBag.Products = produtcs;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice(Invoice invoice, string selectedProducts)
        {
            try
            {
                if (invoice.TotalQuantity == 0)
                {
                    TempData["error"] = "Please select some product before genrating invoice";
                    return RedirectToAction("Index");
                }
                int unique_p = Convert.ToInt32(Request.Form["ProductCount"]);
                IEnumerable<UserInvoiceModel> selectedProductsArray = JsonConvert.DeserializeObject<IEnumerable<UserInvoiceModel>>(selectedProducts);

                invoice.Products = unique_p;
                //invoice.InvoiceNumber = _helperService.GenerateRandomNumber();

                if (ModelState.IsValid)
                {
                    bool result = await _agentService.AddInvoice(invoice);
                    if (result)
                    {
                        bool productAdded = await _agentService.AddInvoiceProducts(selectedProductsArray, invoice.InvoiceNumber);
                        if (productAdded)
                        {
                            TempData["success"] = "Invoice Added Successfully";
                            ViewBag.Invoice = invoice;
                            ViewBag.Products = selectedProductsArray;
                            TempData["invoiceNumber"] = invoice.InvoiceNumber;
                            /*     TempData["Invoice"] = invoice;
                                 TempData["SelectedProducts"] = selectedProductsArray;*/

                            return RedirectToAction("Invoice");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Some error occured while adding invoice";
                        return RedirectToAction("Index");
                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }

        public async Task<IActionResult> Invoice()
        {

            int invoiceNumber = Convert.ToInt32(TempData["invoiceNumber"]);
            Invoice invoice = await _agentService.GetInvoiceByNumber(invoiceNumber);
            IEnumerable<UserInvoiceModel> products = _agentService.GetProductsByInvoiceNumber(invoiceNumber);
            ViewBag.Invoice = invoice;
            ViewBag.Products = products;
            // Pass the invoice object and selected products array to the view
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
