using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
       
        void Commit();
        void Rollback();
    }
}
