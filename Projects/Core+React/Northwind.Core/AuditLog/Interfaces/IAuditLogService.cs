using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.AuditLog.Interfaces
{
    public interface IAuditLogService
    {
        IEnumerable<Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        int TotalRecord(string searchValue);

        IEnumerable<Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "");

        int GetAdvanceSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter);
        int Add(Entities.AuditLog autitLog);
    }
}
