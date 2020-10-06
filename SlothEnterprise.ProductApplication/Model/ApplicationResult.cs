using System;
using System.Collections.Generic;
using System.Text;
using SlothEnterprise.External;

namespace SlothEnterprise.ProductApplication.Model
{
    public class ApplicationResult : IApplicationResult
    {
        public int? ApplicationId { get; set; } = -1;
        public bool Success { get; set; }
        public IList<string> Errors { get; set; }
    }
}
