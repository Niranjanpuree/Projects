using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class Mapper<Source, Destination> where Source : class
                                            where Destination : class
    {
        public static Destination Map(Source parent)
        {
            var child = Activator.CreateInstance<Destination>();
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            try
            {
                foreach (PropertyInfo parentProperty in parentProperties)
                {
                    // var prop = childProperties.Where(c => c.Name == parentProperty.Name && c.PropertyType == parentProperty.PropertyType).FirstOrDefault();
                    var prop = childProperties.Where(c =>
                            c.Name == parentProperty.Name &&
                            (c.Name == parentProperty.Name || c.PropertyType == parentProperty.PropertyType))
                        .FirstOrDefault();
                    if (prop != null)
                    {
                        Type t = Nullable.GetUnderlyingType(parentProperty.PropertyType) ?? prop.PropertyType;
                        if (t == typeof(DateTime) || parentProperty.PropertyType == typeof(DateTime))
                        {
                            object safeValue = (parentProperty.GetValue(parent) == null) ? null : Convert.ChangeType(parentProperty.GetValue(parent), t);
                            var result = Convert.ToDateTime(safeValue).ToString("MM/dd/yyyy");
                            prop.SetValue(child, result);
                        }
                        else
                        {
                            prop.SetValue(child, parentProperty.GetValue(parent));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return child;
        }
    }
    public class ObjectMapper<Source, Destination> where Source : class
                                            where Destination : class
    {
        public static Destination Map(Source parent)
        {
            var child = Activator.CreateInstance<Destination>();
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            try
            {
                foreach (PropertyInfo parentProperty in parentProperties)
                {
                    var prop = childProperties.Where(c => c.Name == parentProperty.Name && c.PropertyType == parentProperty.PropertyType).FirstOrDefault();

                    if (prop != null)
                    {
                        prop.SetValue(child, parentProperty.GetValue(parent));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return child;
        }
    }
}
