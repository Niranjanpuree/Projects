using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ChangeRequestAssignmentStaging> ChangeRequestAssignmentStagings_AssignedToUserId { get; set; }
        public ICollection<ChangeRequestAssignmentStaging> ChangeRequestAssignmentStagings_AssignedByUserId { get; set; }
        public ICollection<ChangeRequestAssignment> ChangeRequestAssignments_AssignedToUserId { get; set; }
        public ICollection<ChangeRequestAssignment> ChangeRequestAssignments_AssignedByUserId { get; set; }
        public ICollection<UserRoleAssignment> UserRoleAssignments { get; set; }
    }
}
