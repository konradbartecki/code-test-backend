using System;
using System.Collections.Generic;
using System.Text;

namespace SlothEnterprise.ProductApplication.Model
{
    public class ConfidentialInvoiceParameters
    {
        public decimal InvoiceLedgerTotalValue { get; set; }
        public decimal AdvantagePercentage { get; set; }
        public decimal VatRate { get; set; }
    }
}
