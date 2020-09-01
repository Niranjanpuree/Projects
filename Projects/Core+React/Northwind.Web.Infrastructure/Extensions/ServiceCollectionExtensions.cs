using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Northwind.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RemoveRegistration<T>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);
            return services;
        }

        public static IServiceCollection ReplaceRegistration<TService,TImplementation>(this IServiceCollection services, ServiceLifetime lifeTime)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TService));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);
            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifeTime);
            services.Add(descriptorToAdd);
            return services;
        }

       

    }
}
