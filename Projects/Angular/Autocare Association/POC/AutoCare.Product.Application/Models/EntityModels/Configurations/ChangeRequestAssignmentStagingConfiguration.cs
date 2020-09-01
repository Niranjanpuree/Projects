using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestAssignmentStagingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestAssignmentStaging>
    {
        public ChangeRequestAssignmentStagingConfiguration(): this("dbo")
        {

        }

        public ChangeRequestAssignmentStagingConfiguration(string schema)
        {
            ToTable(schema + ".ChangeRequestAssignmentStaging");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.AssignedToRoleId).HasColumnName("AssignedToRoleId").IsRequired().HasColumnType("smallint");
            Property(x => x.AssignedToUserId).HasColumnName("AssignedToUserId").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.AssignedByUserId).HasColumnName("AssignedByUserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Comments).HasColumnName("Comments").IsRequired().HasColumnType("nvarchar(max)");
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequestStaging)
               .WithMany(t => t.ChangeRequestAssignmentStagings)
               .HasForeignKey(t => t.ChangeRequestId);

            HasRequired(t => t.AssignedByUser)
               .WithMany(t => t.ChangeRequestAssignmentStagings_AssignedByUserId)
               .HasForeignKey(t => t.AssignedByUserId).WillCascadeOnDelete(false);

            HasRequired(t => t.AssignedToUser)
               .WithMany(t => t.ChangeRequestAssignmentStagings_AssignedToUserId)
               .HasForeignKey(t => t.AssignedToUserId).WillCascadeOnDelete(false);

            HasRequired(t => t.AssignedToRole)
               .WithMany(t => t.ChangeRequestAssignmentStagings)
               .HasForeignKey(t => t.AssignedToRoleId);
        }
    }
}
