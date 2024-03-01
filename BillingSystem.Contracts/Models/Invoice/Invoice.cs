using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts.Models.Invoice
{
    public class Invoice
    {
        public int ID { get; set; }
        public string AgentName { get; set; }

        [Required(ErrorMessage ="Please select a customer befor proceeding")]
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }= DateTime.Now;
        public int TotalAmount { get; set; }
        public int GrandTotal { get; set; }
        public int Discount { get; set; }
        public int Tax { get; set; }
        public string Status { get; set; } = "Pending";
        public int Products { get; set; }
        public int InvoiceNumber { get; set; } = 0;
        public int TotalQuantity { get; set; }

    }
}
