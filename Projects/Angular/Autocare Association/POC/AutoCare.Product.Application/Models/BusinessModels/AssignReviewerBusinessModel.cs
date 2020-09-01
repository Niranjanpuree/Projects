using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class AssignReviewerBusinessModel
    {
        public List<long> ChangeRequestIds { get; set; }
        public string AssignedToUserId { get; set; }
        public short AssignedToRoleId { get; set; }
        public string AssignedByUserId { get; set; }
    }
}
