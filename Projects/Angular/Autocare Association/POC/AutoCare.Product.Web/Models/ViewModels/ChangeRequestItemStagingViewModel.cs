using System;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ChangeRequestItemStagingViewModel
    {
        public long ChangeRequestId { get; set; }
        public long ChangeRequestItemId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Payload { get; set; }
        public ChangeType ChangeType { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual ChangeRequestStaging ChangeRequest { get; set; }
    }
}