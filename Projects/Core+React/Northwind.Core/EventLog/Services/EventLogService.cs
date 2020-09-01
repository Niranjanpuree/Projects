using Northwind.Core.AuditLog.Entities;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.AuditLog.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _eventLogRepository;

        public EventLogService(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }
        public int Add(EventLogs eventLog)
        {
            return _eventLogRepository.Add(eventLog);
        }

        public int GetAdvanceSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter)
        {
            return _eventLogRepository.GetAdvanceSearchCount(searchValue, postValue,userGuid,additionalFilter);
        }

        public IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return _eventLogRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir);
        }

        public IEnumerable<EventLogs> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir, List<AdvancedSearchRequest> postValue, string additionalFilter = "")
        {
            return _eventLogRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir, postValue, additionalFilter);
        }

        public int TotalRecord(string searchValue)
        {
            return _eventLogRepository.TotalRecord(searchValue);
        }

        public EventLogs GetDetails(Guid EventGuid)
        {
            return _eventLogRepository.GetDetails(EventGuid);
        }
    }
}
