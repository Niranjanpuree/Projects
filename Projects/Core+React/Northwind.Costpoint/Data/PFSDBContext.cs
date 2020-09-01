using Northwind.CostPoint.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace NorthWind.Costpoint.Data
{
    public class PFSDBContext: IPFSDBContext
    {
        public string ConnectionString { get; set; }

        private IDbConnection _connection;
        private bool disposed = false;

        public PFSDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(ConnectionString);
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }
                disposed = true;
            }
        }
    }
    
}
