using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
