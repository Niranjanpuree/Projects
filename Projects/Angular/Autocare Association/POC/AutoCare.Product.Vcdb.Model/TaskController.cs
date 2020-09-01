using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TaskController
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public string RequestedBy { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime CompletededDate { get; set; }
        public ChangeRequestStatus Status { get; set; }

        public ICollection<ChangeRequestStaging> ChangeRequestStagings { get; set; }

        public ICollection<ChangeRequestStore> ChangeRequestStores { get; set; }
    }
}
