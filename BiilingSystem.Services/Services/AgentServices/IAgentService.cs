using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiilingSystem.Services.Services.AgentServices
{
    public interface IAgentService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> AddInvoice(Invoice invoice);
        Task<bool>AddInvoiceProducts(IEnumerable<UserInvoiceModel> invoices,int invoiceNumber);
        Task<Invoice> GetInvoiceByNumber(int invoiceNumber);
        public IEnumerable<UserInvoiceModel> GetProductsByInvoiceNumber(int invoiceNumber);
    }
}
