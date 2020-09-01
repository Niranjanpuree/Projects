using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class UserRoleAssignmentConfiguration: System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserRoleAssignment>
    {
        public UserRoleAssignmentConfiguration(): this("dbo")
        {

        }

        public UserRoleAssignmentConfiguration(string schema)
        {
            ToTable("UserRoleAssignment");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RoleId).HasColumnName("RoleId").IsRequired().HasColumnType("smallint");
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");
            Property(x => x.UpdatedDateTime).HasColumnName("UpdatedDatetime").HasColumnType("datetime");
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired().HasColumnType("bit");

            //Relationship
            HasRequired(t => t.Role)
               .WithMany(t => t.UserRoleAssignments)
               .HasForeignKey(t => t.RoleId);


            HasRequired(t => t.User)
               .WithMany(t => t.UserRoleAssignments)
               .HasForeignKey(t => t.UserId);

        }
    }
}
