using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public static class KeyValueHelper
    {
        public static IDictionary<string, string> getGender()
        {
            IDictionary<string, string> model = new Dictionary<string, string>();
            model.Add(new KeyValuePair<string, string>("Male", "Male"));
            model.Add(new KeyValuePair<string, string>("Female", "Female"));
            model.Add(new KeyValuePair<string, string>("Others", "Others"));
            return model;
        }
        public static IDictionary<bool, string> GetYesNo()
        {
            IDictionary<bool, string> model = new Dictionary<bool, string>();
            model.Add(new KeyValuePair<bool, string>(true, "Yes"));
            model.Add(new KeyValuePair<bool, string>(false, "No"));
            return model;
        }
    }
}
