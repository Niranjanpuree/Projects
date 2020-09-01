using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IResourceRepository
    {
        IEnumerable<Resource> GetAll(); 
    }
}
