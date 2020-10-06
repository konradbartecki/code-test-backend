using System;
using System.Collections.Generic;
using System.Text;
using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Model
{
    /// <summary>
    /// Marker interface for IoC registration
    /// </summary>
    public interface IProductApplicationHandler<T> : IProductApplicationHandler where T : IProduct
    {
    }

    public interface IProductApplicationHandler
    {
        IApplicationResult SubmitApplicationFor(ISellerApplication application);
    }
}
