using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestStagingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestStaging>
    {
        public ChangeRequestStagingConfiguration()
            :this("dbo")
        {
            
        }

        public ChangeRequestStagingConfiguration(string schema)
        {
            ToTable(schema + ".ChangeRequestStaging");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ChangeRequestTypeId).HasColumnName("ChangeRequestTypeId").IsRequired().HasColumnType("smallint");
            Property(x => x.Entity).HasColumnName("Entity").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.EntityId).HasColumnName("EntityId").IsRequired().HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.ChangeType).HasColumnName("ChangeType").IsRequired().HasColumnType("smallint");
            Property(x => x.TaskControllerId).HasColumnName("TaskControllerId").HasColumnType("int");
            Property(x => x.RequestedBy).HasColumnName("RequestedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");

            ////Relationship
            //HasOptional(t => t.TaskController)
            //    .WithMany(t => t.ChangeRequestStagings)
            //    .HasForeignKey(t => t.TaskControllerId);
        }
    }
}
