using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Infrastructure.Data
{
    public class NaicsRepository : INaicsRepository
    {
        IDatabaseContext _context;

        public NaicsRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public Naics GetNaicsByCode(string code)
        {
            var sql = @"SELECT TOP(1) *
                          FROM [dbo].[NAICS]
                          WHERE Code=@Code
                      ";
            return _context.Connection.QueryFirstOrDefault<Naics>(sql, new { Code = code });
        }

        public Naics GetNaicsByGuid(Guid naicsGuid)
        {
            var sql = @"SELECT TOP(1) *
                          FROM [dbo].[NAICS]
                          WHERE NAICSGuid=@NAICSGuid
                      ";
            return _context.Connection.QueryFirstOrDefault<Naics>(sql, new { NAICSGuid = naicsGuid });
        }

        public IEnumerable<Naics> GetNaicsList()
        {
            var sql = @"SELECT *
                          FROM [dbo].[NAICS] Order By Code asc
                      ";
            return _context.Connection.Query<Naics>(sql);
        }

        public ICollection<Naics> GetNaicsList(string searchText)
        {
            var nAicsCodeQuery = string.Format($@"
			select * from NAICS
				where code like '%{@searchText}%' or title like '%{@searchText}%' ");
            var nAicsCode = _context.Connection.Query<Naics>(nAicsCodeQuery).ToList();
            return nAicsCode;
        }
    }
}
