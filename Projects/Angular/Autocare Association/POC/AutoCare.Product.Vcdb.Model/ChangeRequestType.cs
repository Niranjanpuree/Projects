using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestType
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ChangeRequestRolesAssignment> ChangeRequestRolesAssignments { get; set; }
    }
}
