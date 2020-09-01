using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    /// <summary>
    /// Use this to mark the status of the entity
    /// </summary>
    public enum EntityStatus
    {        
        InActive = 0,
        Active = 1,
        MarkedForDelete = 2,
    }
}
