using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelDeliveryConfigConfiguration : EntityTypeConfiguration<FuelDeliveryConfig>
    {
        public FuelDeliveryConfigConfiguration()
            : this("dbo")
        {
        }

        public FuelDeliveryConfigConfiguration(string schema)
        {
            ToTable("FuelDeliveryConfig", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelDeliveryConfigID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelDeliveryTypeId).HasColumnName(@"FuelDeliveryTypeID").IsRequired().HasColumnType("int");
            Property(x => x.FuelDeliverySubTypeId).HasColumnName(@"FuelDeliverySubTypeID").IsRequired().HasColumnType("int");
            Property(x => x.FuelSystemControlTypeId).HasColumnName(@"FuelSystemControlTypeID").IsRequired().HasColumnType("int");
            Property(x => x.FuelSystemDesignId).HasColumnName(@"FuelSystemDesignID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.FuelDeliverySubType).WithMany(b => b.FuelDeliveryConfigs).HasForeignKey(c => c.FuelDeliverySubTypeId).WillCascadeOnDelete(false); // FK_FuelDeliverySubType_FuelDeliveryConfig
            HasRequired(a => a.FuelDeliveryType).WithMany(b => b.FuelDeliveryConfigs).HasForeignKey(c => c.FuelDeliveryTypeId).WillCascadeOnDelete(false); // FK_FuelDeliveryType_FuelDeliveryConfig
            HasRequired(a => a.FuelSystemControlType).WithMany(b => b.FuelDeliveryConfigs).HasForeignKey(c => c.FuelSystemControlTypeId).WillCascadeOnDelete(false); // FK_FuelSystemControlType_FuelDeliveryConfig
            HasRequired(a => a.FuelSystemDesign).WithMany(b => b.FuelDeliveryConfigs).HasForeignKey(c => c.FuelSystemDesignId).WillCascadeOnDelete(false); // FK_FuelSystemDesign_FuelDeliveryConfig

            Ignore(x => x.EngineConfigCount);
        }
    }
}
