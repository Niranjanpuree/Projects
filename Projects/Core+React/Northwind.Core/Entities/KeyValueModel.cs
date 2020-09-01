using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public partial struct KeyValuePairModel<Key, Value>
    {
        public Key Keys { get; set; }
        public Value Values { get; set; }
    }

    public partial struct KeyValuePairWithDescriptionModel<Key, Value,Description>
    {
        public Key Keys { get; set; }
        public Value Values { get; set; }
        public Description Descriptions { get; set; }
    }

}
