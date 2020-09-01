using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Role
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ChangeRequestAssignmentStaging> ChangeRequestAssignmentStagings { get; set; }
        public ICollection<ChangeRequestAssignment> ChangeRequestAssignments { get; set; }
        public ICollection<UserRoleAssignment> UserRoleAssignments { get; set; } 
    }
}
