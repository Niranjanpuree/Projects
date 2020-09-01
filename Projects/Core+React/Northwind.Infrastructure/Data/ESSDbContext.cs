using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace Northwind.Infrastructure.Data
{
    public class ESSDbContext : IDatabaseContext
    {
        public string ConnectionString { get; set; }
        
        private IDbConnection _connection;
        private bool disposed = false; 
        
        public ESSDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(ConnectionString);          
        }

        public IDbConnection Connection { get
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

    public class ESSSingletonDbContext : IDatabaseSingletonContext
    {
        public string ConnectionString { get; set; }

        private IDbConnection _connection;
        private bool disposed = false;

        public ESSSingletonDbContext(string connectionString)
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
