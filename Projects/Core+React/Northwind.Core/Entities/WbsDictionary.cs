using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class WbsDictionary
    {
        public Guid WbsDictionaryGuid { get; set; }
        public string ProjectNumber { get; set; }
        public string WbsNumber { get; set; }
        public string WbsDictionaryTitle { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
