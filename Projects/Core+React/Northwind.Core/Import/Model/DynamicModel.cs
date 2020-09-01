using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DynamicModel
    {
        Dictionary<string, object> dynamicProperties = new Dictionary<string, object>();

        public void AddProperty(string key, object value)
        {
            dynamicProperties[key] = value;
        }
    }
}
