using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.AuditLog.Entities;
using Northwind.Core.Models;

namespace Northwind.Core.AuditLog.Interfaces
{
    public interface IEventLogRepository
    {
        IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        int TotalRecord(string searchValue);
        EventLogs GetDetails(Guid EventGuid);
        IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "");
        int GetAdvanceSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter);
        int Add(EventLogs eventLog);
    }
}
