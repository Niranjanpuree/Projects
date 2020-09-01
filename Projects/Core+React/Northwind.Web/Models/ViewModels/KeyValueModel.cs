using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public partial struct KeyValuePairModel<Key, Value>
    {
        public Key Keys { get; set; }
        public Value Values { get; set; }
    }

    public partial struct KeyValuePairWithDescriptionModel<Key, Value, Description>
    {
        public Key Keys { get; set; }
        public Value Values { get; set; }
        public Description Descriptions { get; set; }
    }
}
