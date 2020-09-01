using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestRolesAssignmentConfiguration: System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestRolesAssignment>
    {
        public ChangeRequestRolesAssignmentConfiguration(): this("dbo")
        {

        }

        public ChangeRequestRolesAssignmentConfiguration(string schema)
        {
            ToTable("ChangeRequestRolesAssignment");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RolesId).HasColumnName("RoleId").IsRequired().HasColumnType("smallint");
            Property(x => x.ChangeRequestTypeId).HasColumnName("ChangeRequestTypeId").IsRequired().HasColumnType("int");
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequestType)
               .WithMany(t => t.ChangeRequestRolesAssignments)
               .HasForeignKey(t => t.ChangeRequestTypeId);
        }
    }
}
