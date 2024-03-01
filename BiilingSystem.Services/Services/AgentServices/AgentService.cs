using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Data;
using BillingSystem.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiilingSystem.Services.Services.AgentServices
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;
        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> AddInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> AddInvoiceProducts(IEnumerable<UserInvoiceModel> invoices, int invoiceNumber)
        {
            foreach (var invoice in invoices)
            {
                UserInvoiceModel model = new UserInvoiceModel
                {
                    Name = invoice.Name,
                    Price = invoice.Price,
                    Quantity = invoice.Quantity,
                    InvoiceNumber = invoiceNumber
                };

                // Add the model to the database context
                _context.UserInvoices.Add(model);
            }

            // Save changes to the database
            int entitiesAffected = await _context.SaveChangesAsync();
            return entitiesAffected > 0;
        }

        public async Task<Invoice> GetInvoiceByNumber(int invoiceNumber)
        {
            var invoice=await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
            return invoice;
        }

        // Function to fetch all added products by invoice number
        public IEnumerable<UserInvoiceModel> GetProductsByInvoiceNumber(int invoiceNumber)
        {
            var products= _context.UserInvoices.Where(p => p.InvoiceNumber == invoiceNumber).ToList();
            return products;
        }
    }
}
