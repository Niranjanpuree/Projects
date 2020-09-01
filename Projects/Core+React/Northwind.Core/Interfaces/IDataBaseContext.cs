using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IDatabaseContext: IDisposable
    {
        string ConnectionString { get; set; }
        IDbConnection Connection { get; }        
    }

    public interface IDatabaseSingletonContext : IDisposable
    {
        string ConnectionString { get; set; }
        IDbConnection Connection { get; }
    }
}
