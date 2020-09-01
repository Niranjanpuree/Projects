using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class AssignReviewerInputModel
    {
        public List<long> ChangeRequestIds { get; set; }
        public string AssignedToUserId { get; set; }
        public short AssignedToRoleId { get; set; }
    }
}