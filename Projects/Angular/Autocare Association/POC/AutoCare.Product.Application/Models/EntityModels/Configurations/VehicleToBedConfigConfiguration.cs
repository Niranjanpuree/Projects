using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToBedConfigConfiguration : EntityTypeConfiguration<VehicleToBedConfig>
    {
        public VehicleToBedConfigConfiguration()
            :this("dbo")
        { }
        public VehicleToBedConfigConfiguration( string schema)
        {
            ToTable(schema + ".VehicleToBedConfig");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("VehicleToBedConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.BedConfigId).HasColumnName("BedConfigId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            //for foreign keys
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToBedConfigs).HasForeignKey(c => c.VehicleId);
            HasRequired(a => a.BedConfig).WithMany(b => b.VehicleToBedConfigs).HasForeignKey(c => c.BedConfigId);
        }
    }
}
