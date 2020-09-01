using Northwind.Web.Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Northwind.Web.Models.ViewModels
{
    public class NotificationViewModel
    {
        public DistributionSelection DistributionSelection { get; set; }
        public string AdditionalNotes { get; set; }
        public Guid ResourceId { get; set; }
        public string NotificationTemplateKey { get; set; }
        public IEnumerable<UserViewModel> IndividualRecipients { get; set; }
    }
    public class NotificationDistributionIndividualViewModel
    {
        public string AdditionalNotes { get; set; }
        public Guid DistributionListGuid { get; set; }
        public string ReceiverDisplayName { get; set; }
        public string AdditionalUsers { get; set; }
        public Guid ResourceId { get; set; }
        public Guid ReceiverGuid { get; set; }
        public string NotificationTemplateKey { get; set; }
    }
}
