using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts.Models.Invoice
{
    public class UserInvoiceModel
    {
        public int ID { get; set; }
        public string Name  { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int InvoiceNumber { get; set; }

    }
}
