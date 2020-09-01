using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// Enumeration that identifies the status code for cref="AuthorizationResponse"
    /// </summary>
    public enum AuthorizationStatusCode
    {        
        // This indicates that all value required to make decision where not present
        MissingAttribute=0,
        
        // This indicates success        
        Ok = 1,

        //Policy or Request has syntax error
        SyntaxError = 2,

        //General purpose processing error
        ProcessingError = 3
    }
}
