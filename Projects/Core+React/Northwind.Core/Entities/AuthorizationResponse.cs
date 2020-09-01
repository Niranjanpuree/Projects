using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class AuthorizationResponse
    {
        public AuthorizationDecision Decision { get; set; }
        public AuthorizationStatus Status { get; set; }
        
    }
}
