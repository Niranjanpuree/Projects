using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class QueryOperatorRepository : IQueryOperatorRepository
    {
        private readonly IDatabaseContext _context; 

        public QueryOperatorRepository(IDatabaseContext databaseContext)
        {
            _context = databaseContext;

        }

        public IEnumerable<QueryOperator> GetAll()
        {
            return _context.Connection.Query<QueryOperator>("SELECT QueryOperatorGuid, Name, Title, Type FROM dbo.QueryOperator ORDER BY Title ASC");
        }
    }
}
