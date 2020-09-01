using Dapper;
using Northwind.Core.AuditLog.Entities;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace Northwind.Infrastructure.AuditLog.Data
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IDatabaseContext _context;
        public AuditLogRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public int Add(Core.AuditLog.Entities.AuditLog autitLog)
        {
            string insertQuery = @"INSERT INTO [dbo].[AuditLog]
                                                                   (
                                                                    AuditGuid,
                                                                    RawData, 
                                                                    TimeStamp,
                                                                    Resource, 
                                                                    ResourceId, 
                                                                    Actor, 
                                                                    IPAddress,
                                                                    Action,
                                                                    ActionId,
                                                                    ActionResult,
                                                                    ActionResultReason,
                                                                    AdditionalInformation,
                                                                    AdditionalInformationURl
                                                                    )
                                  VALUES (
                                                                    @AuditGuid,
                                                                    @RawData, 
                                                                    @TimeStamp,
                                                                    @Resource, 
                                                                    @ResourceId, 
                                                                    @Actor, 
                                                                    @IPAddress,
                                                                    @Action,
                                                                    @ActionId,
                                                                    @ActionResult,
                                                                    @ActionResultReason,
                                                                    @AdditionalInformation,
                                                                    @AdditionalInformationURl
                                                                )";
            return _context.Connection.Execute(insertQuery, autitLog);
        }

        public IEnumerable<Core.AuditLog.Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = string.Empty;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = $@"where (Actor LIKE @searchValue or  Action LIKE @searchValue or Resource LIKE @searchValue or Convert(VARCHAR(10),TimeStamp,103) LIKE @searchValue)";
            }

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "TimeStamp";
                dir = "desc";
                additionalSql = $@"{sortField}";
            }
            else if (!sortField.ToLower().Equals("timestamp"))
            {
                additionalSql = $@"REPLACE({sortField}, ' ', '')";
            }
            else if (sortField.ToLower().Equals("timestamp"))
            {
                additionalSql = $@"Timestamp";
            }

            sql = $@"
                   select top 5000* from(
                      select 
                            AuditLogGuid,
                            RawData, 
                            TimeStamp,
                            Resource, 
                            ResourceId, 
                            Actor, 
                            IPAddress,
                            Action,
                            ActionId,
                            ActionResult,
                            ActionResultReason,
                            AdditionalInformation,
                            AdditionalInformationURl
                  from AuditLog
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY
                  ) auditLog ";

            return _context.Connection.Query<Core.AuditLog.Entities.AuditLog>(sql, new { searchValue = searchString });
        }

        public int TotalRecord(string searchValue)
        {
            var sql = @" select 
				  case when count(1) > 5000  then 5000
                  else count(1) end countField
                  from AuditLog";
            return _context.Connection.ExecuteScalar<int>(sql);
        }

        public IEnumerable<Core.AuditLog.Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "")
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            var sql = string.Empty;

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "TimeStamp";
                dir = "desc";
            }
            var whereEntity = selector.RawSql;

            sql = $@"select 
                            AuditLogGuid,
                            RawData, 
                            TimeStamp,
                            Resource, 
                            ResourceId, 
                            Actor, 
                            IPAddress,
                            Action,
                            ActionId,
                            ActionResult,
                            ActionResultReason,
                            AdditionalInformation,
                            AdditionalInformationURl
                  from AuditLog
                  {whereEntity}
                  ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var auditLogList = _context.Connection.Query<Core.AuditLog.Entities.AuditLog>(sql, selector.Parameters);
            return auditLogList;
        }

        public int GetAdvanceSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            var whereEntity = selector.RawSql;

            var sql = $@"select count(1) from AuditLog {whereEntity}";

            var countAuditLog = _context.Connection.ExecuteScalar<int>(sql, selector.Parameters);
            return countAuditLog;
        }
    }
}
