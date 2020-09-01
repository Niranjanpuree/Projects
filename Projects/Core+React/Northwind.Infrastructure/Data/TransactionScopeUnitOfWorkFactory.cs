using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data
{
    public class TransactionScopeUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new TransactionScopeUnitOfWork();
        }
    }
}
