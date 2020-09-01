using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class EngineConfigConfiguration : EntityTypeConfiguration<EngineConfig>
    {
        public EngineConfigConfiguration()
            : this("dbo")
        {
        }

        public EngineConfigConfiguration(string schema)
        {
            ToTable("EngineConfig", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"EngineConfigID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.EngineDesignationId).HasColumnName(@"EngineDesignationID").IsRequired().HasColumnType("int");
            Property(x => x.EngineVinId).HasColumnName(@"EngineVINID").IsRequired().HasColumnType("int");
            Property(x => x.ValvesId).HasColumnName(@"ValvesID").IsRequired().HasColumnType("int");
            Property(x => x.EngineBaseId).HasColumnName(@"EngineBaseID").IsRequired().HasColumnType("int");
            Property(x => x.FuelDeliveryConfigId).HasColumnName(@"FuelDeliveryConfigID").IsRequired().HasColumnType("int");
            Property(x => x.AspirationId).HasColumnName(@"AspirationID").IsRequired().HasColumnType("int");
            Property(x => x.CylinderHeadTypeId).HasColumnName(@"CylinderHeadTypeID").IsRequired().HasColumnType("int");
            Property(x => x.FuelTypeId).HasColumnName(@"FuelTypeID").IsRequired().HasColumnType("int");
            Property(x => x.IgnitionSystemTypeId).HasColumnName(@"IgnitionSystemTypeID").IsRequired().HasColumnType("int");
            Property(x => x.EngineMfrId).HasColumnName(@"EngineMfrID").IsRequired().HasColumnType("int");
            Property(x => x.EngineVersionId).HasColumnName(@"EngineVersionID").IsRequired().HasColumnType("int");
            Property(x => x.PowerOutputId).HasColumnName(@"PowerOutputID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.Aspiration).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.AspirationId).WillCascadeOnDelete(false); // FK_Aspiration_EngineConfig
            HasRequired(a => a.CylinderHeadType).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.CylinderHeadTypeId).WillCascadeOnDelete(false); // FK_CylinderHeadType_EngineConfig
            HasRequired(a => a.EngineBase).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.EngineBaseId).WillCascadeOnDelete(false); // FK_EngineBase_EngineConfig
            HasRequired(a => a.EngineDesignation).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.EngineDesignationId).WillCascadeOnDelete(false); // FK_EngineDesignation_EngineConfig
            HasRequired(a => a.EngineVersion).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.EngineVersionId).WillCascadeOnDelete(false); // FK_EngineVersion_EngineConfig
            HasRequired(a => a.EngineVin).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.EngineVinId).WillCascadeOnDelete(false); // FK_EngineVIN_EngineConfig
            HasRequired(a => a.FuelDeliveryConfig).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.FuelDeliveryConfigId).WillCascadeOnDelete(false); // FK_FuelDeliveryConfig_EngineConfig
            HasRequired(a => a.FuelType).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.FuelTypeId).WillCascadeOnDelete(false); // FK_FuelType_EngineConfig
            HasRequired(a => a.IgnitionSystemType).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.IgnitionSystemTypeId).WillCascadeOnDelete(false); // FK_IgnitionSystemType_EngineConfig
            HasRequired(a => a.Mfr).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.EngineMfrId).WillCascadeOnDelete(false); // FK_Mfr_EngineConfig
            HasRequired(a => a.PowerOutput).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.PowerOutputId).WillCascadeOnDelete(false);
            HasRequired(a => a.Valves).WithMany(b => b.EngineConfigs).HasForeignKey(c => c.ValvesId).WillCascadeOnDelete(false);

            Ignore(x => x.VehicleToEngineConfigCount);
        }
    }
}
