using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class SyncValidationResult
    {
        public bool IsValid { get; set; }
        public Dictionary<string, object> Errors { get; set; }

        public SyncValidationResult()
        {
            IsValid = true;
            Errors = new Dictionary<string, object>();
        }
    }
}
