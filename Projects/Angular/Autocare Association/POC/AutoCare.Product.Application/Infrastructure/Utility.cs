using System;

namespace AutoCare.Product.Application.Infrastructure
{
    public static class Utility
    {
        public static object GetGenericTypeInstance(Type genericType, Type[] genericConstraints, params object[] arguments)
        {
            var objectType = genericType.MakeGenericType(genericConstraints);
            var instance = Activator.CreateInstance(objectType, arguments);
            return instance;
        }
    }
}
