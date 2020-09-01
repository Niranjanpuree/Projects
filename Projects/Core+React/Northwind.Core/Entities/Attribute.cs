using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// Authorization Decision Enum
    /// </summary>
    public class Attribute
    {
        public Guid AttributeMetaGuid { get; set; }
        public string AttributeName { get; set; }
        public string Resource { get; set; }
        public string DataType { get; set; }
        public string Max { get; set; }
        public string Min { get; set; }
        public bool IsSystem { get; set; }
        public int IsAvailableForSearch { get; set; }
        public string IsReadOnly { get; set; }
        public string ValidationPayload { get; set; }
    }

}
