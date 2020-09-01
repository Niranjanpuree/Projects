using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data
{
    public class TransactionScopeUnitOfWork : IUnitOfWork
    {
       
        private readonly TransactionScope _transactionScope;
        private bool disposed = false;

        public TransactionScopeUnitOfWork()
        {
            _transactionScope = new TransactionScope(TransactionScopeOption.Required,TimeSpan.FromMinutes(10));
            
        }

        public void Commit()
        {
            _transactionScope.Complete();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    _transactionScope.Dispose();
                }
                disposed = true;
            }
        }

        public void Rollback()
        {
            _transactionScope.Dispose();
        }
    }
}
