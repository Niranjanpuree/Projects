using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TaskControllerConfiguration: System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<TaskController>
    {
        public TaskControllerConfiguration()
        {
            
        }

        public TaskControllerConfiguration(string schema)
        {
            ToTable("TaskController");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Entity).HasColumnName("Entity").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.RequestedBy).HasColumnName("RequestedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ReceivedDate).HasColumnName("ReceivedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.CompletededDate).HasColumnName("CompletededDate").HasColumnType("datetime");
            Property(x => x.Status).HasColumnName("Status").IsRequired().HasColumnType("smallint");

            HasMany<ChangeRequestStaging>(t => t.ChangeRequestStagings)
                .WithOptional(t => t.TaskController);

            HasMany<ChangeRequestStore>(t => t.ChangeRequestStores)
                .WithOptional(t => t.TaskController);
        }
    }
}
