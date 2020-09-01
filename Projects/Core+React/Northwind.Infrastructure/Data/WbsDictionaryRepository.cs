using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class WbsDictionaryRepository : IWbsDictionaryRepository
    {
        public IDatabaseContext _context;
        public WbsDictionaryRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(WbsDictionary wbsDictionary)
        {
            var sql = @"INSERT INTO [WbsDictionaries]
           ([ProjectNumber]
           ,[WbsNumber]
           ,[WbsDictionaryTitle]
           ,[CreatedBy]
           ,[CreatedOn])
             VALUES
                   (@ProjectNumber,
                    @WbsNumber,
                    @wbsDictionaryTitle,
                    @CreatedBy,
                    @CreatedOn)";
            _context.Connection.Execute(sql, wbsDictionary);

        }

        public void Delete(Guid wbsDictionaryGuid)
        {
            var sql = @"DELETE FROM [dbo].[WbsDictionaries]
                        WHERE WbsDictionaryGuid=@WbsDictionaryGuid";
            _context.Connection.Execute(sql, new { WbsDictionaryGuid = wbsDictionaryGuid });
        }
        
        public IEnumerable<WbsDictionary> GetWbsDictionary(string ProjectNumber, string WbsNumber, string searchValue, int skip, int take, string orderBy, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND WbsDictionaryTitle LIKE @searchValue ";
            }

            var sql = $@"select * from WbsDictionaries
            Where ProjectNumber=@ProjectNumber and WbsNumber=@WbsNumber {searchParam}  ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<WbsDictionary>(sql, new { ProjectNumber, WbsNumber });
        }

        public WbsDictionary GetWbsDictionaryByGuid(Guid DictionaryGuid)
        {
            var sql = @"SELECT * FROM [dbo].[WbsDictionaries]
                        WHERE WbsDictionaryGuid=@DictionaryGuid";
            var result = _context.Connection.Query<WbsDictionary>(sql, new { DictionaryGuid });
            if (result.AsList().Count > 0)
            {
                return result.AsList()[0];
            }
            else
            {
                return null;
            }
        }

        public int GetWbsDictionaryCount(string ProjectNumber, string WbsNumber, string searchValue)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND WbsDictionaryTitle LIKE @searchValue ";
            }

            var sql = $@"select count(1) from WbsDictionaries
           Where ProjectNumber=@ProjectNumber and WbsNumber=@WbsNumber {searchParam}";
            return _context.Connection.ExecuteScalar<int>(sql, new { ProjectNumber, WbsNumber, searchValue });
        }

        public void Update(WbsDictionary wbsDictionary)
        {
            var sql = @"UPDATE [dbo].[WbsDictionaries]
                           SET [WbsDictionaryTitle] = @WbsDictionaryTitle,
                              [CreatedBy] = @CreatedBy,
                              [CreatedOn] = @CreatedOn
                         WHERE WbsDictionaryGuid=@WbsDictionaryGuid";
            _context.Connection.Execute(sql, wbsDictionary);
        }
    }
}
