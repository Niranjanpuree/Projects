using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestStoreConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestStore>
    {
        public ChangeRequestStoreConfiguration()
            :this("dbo")
        {

        }

        public ChangeRequestStoreConfiguration(string schema)
        {
            ToTable(schema + ".ChangeRequestStore");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ChangeRequestTypeId).HasColumnName("ChangeRequestTypeId").IsRequired().HasColumnType("smallint");
            Property(x => x.Entity).HasColumnName("Entity").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.EntityId).HasColumnName("EntityId").IsRequired().HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.ChangeType).HasColumnName("ChangeType").IsRequired().HasColumnType("smallint");
            Property(x => x.Status).HasColumnName("Status").IsRequired().HasColumnType("smallint");
            Property(x => x.DecisionBy).HasColumnName("DecisionBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.TaskControllerId).HasColumnName("TaskControllerId").HasColumnType("int");
            Property(x => x.RequestedBy).HasColumnName("RequestedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.RequestedDateTime).HasColumnName("RequestedDateTime").IsRequired().HasColumnType("datetime");
            Property(x => x.CompletedDateTime).HasColumnName("CompletedDateTime").HasColumnType("datetime");

            ////Relationship
            //HasOptional(t => t.TaskController)
            //    .WithMany(t => t.ChangeRequestStores)
            //    .HasForeignKey(t => t.TaskControllerId);
        }
    }
}
