using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestAssignment
    {
        public long Id { get; set; }
        public long ChangeRequestId { get; set; }

        public Int16 AssignedToRoleId { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedByUserId { get; set; }
        public string Comments { get; set; } 
        public DateTime CreatedDateTime { get; set; }
        public ChangeRequestStore ChangeRequestStore { get; set; }
        public Role AssignedToRole { get; set; }
        public User AssignedToUser { get; set; }
        public User AssignedByUser { get; set; }
    }
}
