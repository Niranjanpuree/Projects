using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data
{
    public class MsSqlContextFactory : IDatabaseContextFactory
    {
        private readonly string _connectionString;
        public MsSqlContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDatabaseContext Create()
        {
            return new ESSDbContext(_connectionString);
        }
    }

    public class MsSqlSingletonContextFactory : IDatabaseSingletonContextFactory
    {
        private readonly string _connectionString;
        public MsSqlSingletonContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDatabaseContext Create()
        {
            return new ESSDbContext(_connectionString);
        }
    }
}
