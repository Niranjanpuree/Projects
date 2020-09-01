using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class UserSyncValidationResult
    {
        public bool IsValid { get; set; }
        public Dictionary<string, object> Errors {get; set;}
        
        public UserSyncValidationResult()
        {
            IsValid = true;
            Errors = new Dictionary<string, object>();
        }
        
    }
}
