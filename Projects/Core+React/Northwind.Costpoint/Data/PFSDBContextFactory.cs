using Dapper;
using Northwind.CostPoint.Interfaces;

namespace NorthWind.Costpoint.Data
{
    public class PFSDBContextFactory : IPFSDBContextFactory
    {
        private readonly string _connectionString;
        public PFSDBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IPFSDBContext Create()
        {
            return new PFSDBContext(_connectionString);
        }
    }
}
