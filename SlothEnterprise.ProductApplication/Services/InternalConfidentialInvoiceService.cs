using System;
using System.Collections.Generic;
using System.Text;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Model;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Services
{
    public class InternalConfidentialInvoiceService : IProductApplicationHandler<ConfidentialInvoiceDiscount>
    {
        private readonly IConfidentialInvoiceService _inner;

        public InternalConfidentialInvoiceService(IConfidentialInvoiceService inner)
        {
            _inner = inner; 
        }

        public IApplicationResult SubmitApplicationFor(ISellerApplication application)
        {
            var product = (ConfidentialInvoiceDiscount) application.Product;

            var applicantData = new CompanyDataRequest
            {
                CompanyFounded = application.CompanyData.Founded,
                CompanyNumber = application.CompanyData.Number,
                CompanyName = application.CompanyData.Name,
                DirectorName = application.CompanyData.DirectorName
            };

            return _inner.SubmitApplicationFor(applicantData: applicantData,
                invoiceLedgerTotalValue: product.TotalLedgerNetworth,
                advantagePercentage: product.AdvancePercentage,
                vatRate: product.VatRate);
        }
    }
}
