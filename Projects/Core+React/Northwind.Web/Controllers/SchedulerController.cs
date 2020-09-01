using System;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using Northwind.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class SchedulerController : Controller
    {
        private readonly IUserSyncService _userSyncService;
        private readonly IADGroupSyncService _groupSyncService;
        private readonly IActiveDirectoryContext _adContext;
        private readonly ISyncBatchService _syncBatchService;

        public SchedulerController(IUserSyncService userSyncService, IADGroupSyncService groupSyncService, ISyncBatchService syncBatchService, IActiveDirectoryContext adContext)
        {
            _userSyncService = userSyncService;
            _groupSyncService = groupSyncService;
            _syncBatchService = syncBatchService;
            _adContext = adContext;
            
        }
        public IActionResult Index()
        {
            var syncBatch = new SyncBatch();
            syncBatch.BatchStart = DateTime.Now;
            Guid batchGuid = _syncBatchService.Insert(syncBatch);

            //_groupSyncService.SyncGroupsFromActiveDirectory(_adContext, batchGuid);
            //_groupSyncService.SyncGroupUsersAndManagerFromActiveDirectory(_adContext, batchGuid);
            _userSyncService.SyncUsersFromActiveDirectory(_adContext, batchGuid);
            
            
            return View();
        }
    }
}