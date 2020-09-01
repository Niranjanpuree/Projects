using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Import.Interface;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web
{
    public class HangfireScheduler: IHangfireScheduler
    {
        private readonly IUserSyncService _userSyncService;
        private readonly IActiveDirectoryContext _adContext;
        private readonly ISyncBatchService _syncBatchService;
        private readonly IConfiguration _configuration;
        private readonly IImportService _importService;
        private readonly IHttpContextAccessor _context;
        public HangfireScheduler(IUserSyncService userSyncService, ISyncBatchService syncBatchService, IActiveDirectoryContext adContext, IImportService importService,IConfiguration configuration, IHttpContextAccessor context)
        {
            _userSyncService = userSyncService;
            _syncBatchService = syncBatchService;
            _adContext = adContext;
            _importService = importService;
            _configuration = configuration;
            _context = context;
        }

        public void Register()
        {
            //RecurringJob.AddOrUpdate("Active Directory User Sync", () => ActiveDirectoryUserSync(), Cron.Daily,TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate("Data Migration", () => BatchImport(), Cron.Monthly, TimeZoneInfo.Local);
            //BackgroundJob.Enqueue(() => BatchImport());
        }

        public void ActiveDirectoryUserSync()
        {
            var syncBatch = new SyncBatch
            {
                BatchStart = DateTime.Now
            };
            Guid batchGuid = _syncBatchService.Insert(syncBatch);
            _userSyncService.SyncUsersFromActiveDirectory(_adContext, batchGuid);
        }

        public void BatchImport()
        {
            var syncBatch = new SyncBatch
            {
                BatchStart = DateTime.Now
            };
            var c = _context.HttpContext;
            var fileName = _configuration.GetSection("CSVImport").GetValue<string>("fileName");
            var filePath = _configuration.GetSection("CSVImport").GetValue<string>("filePath");
            var errorLogPath = _configuration.GetValue<string>("ImportLogPath");
            var fullFilePath = filePath + fileName;
            var userGuid = Guid.Empty;
            _importService.ImportData(fullFilePath, userGuid, errorLogPath);
        }
    }
}
