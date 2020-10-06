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
    public class InternalBusinessLoanService : IProductApplicationHandler<BusinessLoans>
    {
        private readonly IBusinessLoansService _inner;

        public InternalBusinessLoanService(IBusinessLoansService inner)
        {
            _inner = inner;
        }
        public IApplicationResult SubmitApplicationFor(ISellerApplication application)
        {
            var product = (BusinessLoans) application.Product;
            var loanRequest = new LoansRequest
            {
                InterestRatePerAnnum = product.InterestRatePerAnnum,
                LoanAmount = product.LoanAmount
            };
            var result = _inner.SubmitApplicationFor(new CompanyDataRequest
            {
                CompanyFounded = application.CompanyData.Founded,
                CompanyNumber = application.CompanyData.Number,
                CompanyName = application.CompanyData.Name,
                DirectorName = application.CompanyData.DirectorName
            }, loanRequest);

            return result;
        }
    }
}
