using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToBrakeConfigConfiguration: EntityTypeConfiguration<VehicleToBrakeConfig>
    {
        public VehicleToBrakeConfigConfiguration()
            :this("dbo")
        {
        }

        public VehicleToBrakeConfigConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToBrakeConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("VehicleToBrakeConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.BrakeConfigId).HasColumnName("BrakeConfigId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);

            // Foreign keys
            HasRequired(a => a.BrakeConfig).WithMany(b => b.VehicleToBrakeConfigs).HasForeignKey(c => c.BrakeConfigId);
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToBrakeConfigs).HasForeignKey(c => c.VehicleId);
        }
    }
}
