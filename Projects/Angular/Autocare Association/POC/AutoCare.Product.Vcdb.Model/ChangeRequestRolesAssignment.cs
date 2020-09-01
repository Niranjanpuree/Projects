using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestRolesAssignment
    {
        public int Id { get; set; }
        public short ChangeRequestTypeId { get; set; }
        public short RolesId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public ChangeRequestType ChangeRequestType { get; set; }
    }
}
