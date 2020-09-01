using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class UserRoleAssignment
    {
        public int Id { get; set; }
        public short RoleId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
