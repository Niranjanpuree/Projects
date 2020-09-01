using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IValidationHelper
    {
        bool ReservedChar(string reserve);
        bool CheckNull(string reqString);
    }
}
