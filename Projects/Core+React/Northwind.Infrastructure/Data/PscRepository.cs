using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data
{
    public class PscRepository: IPscRepository
    {
        IDatabaseContext _context;

        public PscRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Psc> GetPscByCode(string code)
        {
            var sql = @"SELECT [PSCGuid]
                              ,[CodeDescription]
                              ,[Code]
                              ,[Level1]
                              ,[Level1Category]
                              ,[Level2]
                              ,[Level2Category]
                          FROM [dbo].[PSC]
                          WHERE Code=@Code
                      ";
            return _context.Connection.Query<Psc>(sql, new { Code = code });
        }

        public IEnumerable<Psc> GetPscByGuid(Guid pscGuid)
        {
            var sql = @"SELECT [PSCGuid]
                              ,[CodeDescription]
                              ,[Code]
                              ,[Level1]
                              ,[Level1Category]
                              ,[Level2]
                              ,[Level2Category]
                          FROM [dbo].[PSC]
                          WHERE PSCGuid=@PSCGuid
                      ";
            return _context.Connection.Query<Psc>(sql, new { PSCGuid = pscGuid });
        }

        public IEnumerable<Psc> GetPscs()
        {
            var sql = @"SELECT [PSCGuid]
                              ,[CodeDescription]
                              ,[Code]
                              ,[Level1]
                              ,[Level1Category]
                              ,[Level2]
                              ,[Level2Category]
                          FROM [dbo].[PSC] Order By Code asc
                      ";
            return _context.Connection.Query<Psc>(sql);
        }

        public Psc GetPSCDetailByCode(string code)
        {
            var sql = @"SELECT * 
                        FROM [dbo].[PSC]
                        WHERE Code = @code";
            return _context.Connection.QueryFirstOrDefault<Psc>(sql, new { code = code });
        }

        public IEnumerable<Psc> GetPscList(string searchText)
        {
            var pScCodeQuery = string.Format($@"
			SELECT * FROM PSC
				WHERE code like '%{@searchText}%' or CodeDescription like '%{@searchText}%' ");
            var pScSCode = _context.Connection.Query<Psc>(pScCodeQuery).ToList();
            return pScSCode;
        }
    }
}
