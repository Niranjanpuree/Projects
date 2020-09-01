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
    public class EventLogRepository : IEventLogRepository
    {
        private readonly IDatabaseContext _context;
        public EventLogRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public int Add(EventLogs eventLog)
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
            return _context.Connection.Execute(insertQuery, eventLog);
        }

        public IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = string.Empty;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = $@"where (Application LIKE @searchValue or Message LIKE @searchValue or  Action LIKE @searchValue or Resource LIKE @searchValue or Convert(VARCHAR(10),EventDate,103) LIKE @searchValue)";
            }

            if (string.IsNullOrEmpty(sortField) || sortField == "userName")
            {
                sortField = "EventDate";
                dir = "desc";
                additionalSql = $@"{sortField}";
            }
            else if (!sortField.ToLower().Equals("eventdate"))
            {
                additionalSql = $@"REPLACE({sortField}, ' ', '')";
            }
            else if (sortField.ToLower().Equals("eventdate"))
            {
                additionalSql = $@"EventDate";
            }

            sql = $@"
                   select top 5000* from(
                      select 
                           *
                  from EventLog
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY
                  ) auditLog ";

            return _context.Connection.Query<EventLogs>(sql, new { searchValue = searchString });
        }

        public int TotalRecord(string searchValue)
        {
            var sql = @" select 
				  case when count(1) > 5000  then 5000
                  else count(1) end countField
                  from EventLog";
            return _context.Connection.ExecuteScalar<int>(sql);
        }

        public EventLogs GetDetails(Guid EventGuid)
        {
            var sql = $@"select * from EventLog where EventGuid = @EventGuid";
            return _context.Connection.QueryFirst<EventLogs>(sql, new { EventGuid = @EventGuid });
        }

        public IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "")
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

            if (string.IsNullOrEmpty(sortField) || sortField=="userName")
            {
                sortField = "EventDate";
                dir = "desc";
            }
            var whereEntity = selector.RawSql;

            sql = $@"select 
                            *
                  from EventLog
                  {whereEntity}
                  ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var auditLogList = _context.Connection.Query<EventLogs>(sql, selector.Parameters);
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

            var sql = $@"select count(1) from EventLog {whereEntity}";

            var countAuditLog = _context.Connection.ExecuteScalar<int>(sql, selector.Parameters);
            return countAuditLog;
        }
    }
}
