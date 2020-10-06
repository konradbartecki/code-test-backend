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
    public class InternalSelectInvoiceService : IProductApplicationHandler<SelectiveInvoiceDiscount>
    {
        private readonly ISelectInvoiceService _inner;

        public InternalSelectInvoiceService(ISelectInvoiceService inner)
        {
            _inner = inner;
        }
        public IApplicationResult SubmitApplicationFor(ISellerApplication application)
        {
            IApplicationResult result = new ApplicationResult();
            if (application.CompanyData.Number == default)
            {
                result.Errors.Add("Company number was not provided");
                return result;
            }

            var product = (SelectiveInvoiceDiscount) application.Product;

            var appId = _inner.SubmitApplicationFor(
                companyNumber: application.CompanyData.Number.ToString(),
                invoiceAmount: product.InvoiceAmount,
                advancePercentage: product.AdvancePercentage);

            bool isSuccess = appId != -1;
            result.ApplicationId = appId;
            result.Success = isSuccess;

            return result;
        }
    }
}
