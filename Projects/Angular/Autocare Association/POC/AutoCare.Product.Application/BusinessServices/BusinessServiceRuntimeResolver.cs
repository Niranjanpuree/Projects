using System;
using Microsoft.Practices.Unity;

namespace AutoCare.Product.Application.BusinessServices
{
    public class BusinessServiceRuntimeResolver : IBusinessServiceRuntimeResolver
    {
        private readonly IUnityContainer _container;

        public BusinessServiceRuntimeResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object Resolve(string typeName)
        {
            return _container.Resolve(Type.GetType(typeName));
        }
    }
}
