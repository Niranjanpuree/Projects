using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// Authorization Decision Enum
    /// </summary>
    public enum AuthorizationDecision
    {       
        Deny=0,
        Permit=1,
        Indeterminant=2,
        NotApplicable=3,
    }
}
