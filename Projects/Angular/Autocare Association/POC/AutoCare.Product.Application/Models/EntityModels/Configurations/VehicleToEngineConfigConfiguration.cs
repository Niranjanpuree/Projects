using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToEngineConfigConfiguration : EntityTypeConfiguration<VehicleToEngineConfig>
    {
        public VehicleToEngineConfigConfiguration()
            : this("dbo")
        {
        }

        public VehicleToEngineConfigConfiguration(string schema)
        {
            ToTable("VehicleToEngineConfig", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"VehicleToEngineConfigID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName(@"VehicleID").IsRequired().HasColumnType("int");
            Property(x => x.EngineConfigId).HasColumnName(@"EngineConfigID").IsRequired().HasColumnType("int");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.EngineConfig).WithMany(b => b.VehicleToEngineConfigs).HasForeignKey(c => c.EngineConfigId).WillCascadeOnDelete(false); // engineconfigvehicleto_fk
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToEngineConfigs).HasForeignKey(c => c.VehicleId).WillCascadeOnDelete(false); // vehicletoengineconfigvehicle_fk
        }
    }
}
