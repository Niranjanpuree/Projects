using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class ValidationResult
    {

        public ValidationStatus StatusName { get; set; }
        public Dictionary<string, object> Message { get; set; }
        public ValidationResult()
        {
            StatusName  = ValidationStatus.Fail;
            Message = new Dictionary<string, object>();
        }
    }
}
