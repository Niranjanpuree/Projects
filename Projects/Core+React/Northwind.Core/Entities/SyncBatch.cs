using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class SyncBatch
    {
        public Guid SyncBatchGUID { get; set; }
        public DateTime BatchStart { get; set; }
        public DateTime BatchEnd { get; set; }
    }
}
