using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BrakeConfigConfiguration: EntityTypeConfiguration<BrakeConfig>
    {
        public BrakeConfigConfiguration()
            :this("dbo")
        {
        }

        public BrakeConfigConfiguration(string schema)
        {
            ToTable(schema + ".BrakeConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BrakeConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FrontBrakeTypeId).HasColumnName("FrontBrakeTypeId").IsRequired().HasColumnType("int");
            Property(x => x.RearBrakeTypeId).HasColumnName("RearBrakeTypeId").IsRequired().HasColumnType("int");
            Property(x => x.BrakeSystemId).HasColumnName("BrakeSystemId").IsRequired().HasColumnType("int");
            Property(x => x.BrakeABSId).HasColumnName("BrakeABSId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToBrakeConfigCount);

            // Foreign keys
            HasRequired(a => a.BrakeABS).WithMany(b => b.BrakeConfigs).HasForeignKey(c => c.BrakeABSId);
            HasRequired(a => a.BrakeSystem).WithMany(b => b.BrakeConfigs).HasForeignKey(c => c.BrakeSystemId);
            HasRequired(a => a.FrontBrakeType).WithMany(b => b.BrakeConfigs_FrontBrakeTypeId).HasForeignKey(c => c.FrontBrakeTypeId).WillCascadeOnDelete(false);
            HasRequired(a => a.RearBrakeType).WithMany(b => b.BrakeConfigs_RearBrakeTypeId).HasForeignKey(c => c.RearBrakeTypeId).WillCascadeOnDelete(false);
        }
    }
}
