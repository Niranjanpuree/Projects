using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public class MyAntiforgeryService : Attribute, IFilterFactory, IFilterMetadata, IOrderedFilter
    {
        public bool IsReusable => false;

        public int Order => 1;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
