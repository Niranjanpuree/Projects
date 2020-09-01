using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IQueryOperatorRepository
    {
        IEnumerable<QueryOperator> GetAll();
    }
}
