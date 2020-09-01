using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToSteeringConfigConfiguration : EntityTypeConfiguration<VehicleToSteeringConfig>
    {
        public VehicleToSteeringConfigConfiguration()
            :this("dbo")
        {
        }

        public VehicleToSteeringConfigConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToSteeringConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("VehicleToSteeringConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.SteeringConfigId).HasColumnName("SteeringConfigId").IsRequired().HasColumnType("int");
            //Property(x => x.Source).HasColumnName("Source");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.SteeringConfig).WithMany(b => b.VehicleToSteeringConfigs).HasForeignKey(c => c.SteeringConfigId);
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToSteeringConfigs).HasForeignKey(c => c.VehicleId);
        }
    }
}
