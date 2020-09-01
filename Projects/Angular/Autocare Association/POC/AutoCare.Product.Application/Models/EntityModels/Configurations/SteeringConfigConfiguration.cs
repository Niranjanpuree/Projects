using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SteeringConfigConfiguration : EntityTypeConfiguration<SteeringConfig>
    {
        public SteeringConfigConfiguration()
            :this("dbo")
        {
        }

        public SteeringConfigConfiguration(string schema)
        {
            ToTable(schema + ".SteeringConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SteeringConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.SteeringTypeId).HasColumnName("SteeringTypeId").IsRequired().HasColumnType("int");
           
            Property(x => x.SteeringSystemId).HasColumnName("SteeringSystemId").IsRequired().HasColumnType("int");
           
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToSteeringConfigCount);

            // Foreign keys
           
            HasRequired(a => a.SteeringSystem).WithMany(b => b.SteeringConfigs).HasForeignKey(c => c.SteeringSystemId);
            HasRequired(a => a.SteeringType).WithMany(b => b.SteeringConfigs_SteeringTypeId).HasForeignKey(c => c.SteeringTypeId).WillCascadeOnDelete(false);
            
        }
    }
}
