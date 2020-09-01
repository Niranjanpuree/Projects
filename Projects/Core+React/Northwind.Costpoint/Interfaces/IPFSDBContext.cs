using System;
using System.Data;

namespace Northwind.CostPoint.Interfaces
{
    public interface IPFSDBContext: IDisposable
    {
        string ConnectionString { get; set; }
        IDbConnection Connection { get; }
    }

}
