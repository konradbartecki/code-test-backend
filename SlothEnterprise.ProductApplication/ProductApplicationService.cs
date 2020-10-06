using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Model;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.Services;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly ServiceProvider _serviceProvider;


        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            var quickIoC = new ServiceCollection();
            quickIoC.AddScoped(x => selectInvoiceService);
            quickIoC.AddScoped(x => confidentialInvoiceWebService);
            quickIoC.AddScoped(x => businessLoansService);

            quickIoC.AddScoped<IProductApplicationHandler<SelectiveInvoiceDiscount>, InternalSelectInvoiceService>();
            quickIoC.AddScoped<IProductApplicationHandler<ConfidentialInvoiceDiscount>, InternalConfidentialInvoiceService>();
            quickIoC.AddScoped<IProductApplicationHandler<BusinessLoans>, InternalBusinessLoanService>();
            _serviceProvider = quickIoC.BuildServiceProvider();
        }

        private IProductApplicationHandler GetProductHandler(IProduct product)
        {
            if (product == null) throw new InvalidOperationException();

            var productType = product.GetType();
            var productHandlerType = typeof(IProductApplicationHandler<>).MakeGenericType(productType);
            var service = (IProductApplicationHandler) _serviceProvider.GetRequiredService(productHandlerType);
            return service;
        }

        public IApplicationResult SubmitApplicationForV2(ISellerApplication application)
        {
            var handler = GetProductHandler(application.Product);
            return handler.SubmitApplicationFor(application);
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            var handler = GetProductHandler(application.Product);
            return handler.SubmitApplicationFor(application)
                .ApplicationId.GetValueOrDefault();
            //Backward compatibility for SelectiveInvoiceDiscount
            //Other handlers will return either application id or -1 on error
        }
    }
}
