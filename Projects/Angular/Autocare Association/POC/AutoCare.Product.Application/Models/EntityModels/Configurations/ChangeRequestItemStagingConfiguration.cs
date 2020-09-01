using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestItemStagingConfiguration: System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestItemStaging>
    {
        public ChangeRequestItemStagingConfiguration(): this("dbo")
        {
                
        }

        public ChangeRequestItemStagingConfiguration(string schema)
        {
            ToTable("ChangeRequestItemStaging");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("ChangeRequestItemId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.Entity).HasColumnName("Entity").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.EntityFullName).HasColumnName("EntityFullName").IsRequired().HasColumnType("varchar").HasMaxLength(1024);
            Property(x => x.EntityId).HasColumnName("EntityId").IsRequired().HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Payload).HasColumnName("Payload").IsRequired().HasColumnType("varchar(max)");
            Property(x => x.ExistingPayload).HasColumnName("ExistingPayload").HasColumnType("varchar(max)");
            Property(x => x.ChangeType).HasColumnName("ChangeType").IsRequired().HasColumnType("smallint");
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequest)
                .WithMany(t => t.ChangeRequestItemStagings)
                .HasForeignKey(t => t.ChangeRequestId);
        }
    }
}
