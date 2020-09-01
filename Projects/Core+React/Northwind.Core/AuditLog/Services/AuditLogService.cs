using Northwind.Core.AuditLog.Entities;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.AuditLog.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }
        public int Add(Entities.AuditLog auditLog)
        {
            return _auditLogRepository.Add(auditLog);
        }

        public int GetAdvanceSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter)
        {
            return _auditLogRepository.GetAdvanceSearchCount(searchValue, postValue,userGuid,additionalFilter);
        }

        public IEnumerable<Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return _auditLogRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir);
        }

        public IEnumerable<Entities.AuditLog> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "")
        {
            return _auditLogRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir, postValue, additionalFilter);
        }

        public int TotalRecord(string searchValue)
        {
            return _auditLogRepository.TotalRecord(searchValue);
        }
    }
}
