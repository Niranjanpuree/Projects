using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class SyncStatus
    {
        public Guid SyncGUID { get; set; }
        public Guid SyncBatchGUID { get; set; }
        public string SyncStatusText { get; set; }
        public string ErrorMessage { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public Guid ObjectGUID { get; set; }
    }
}
