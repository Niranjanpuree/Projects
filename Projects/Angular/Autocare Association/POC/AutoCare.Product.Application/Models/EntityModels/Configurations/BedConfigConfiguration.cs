using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BedConfigConfiguration:EntityTypeConfiguration<BedConfig>
    {
        public BedConfigConfiguration()
            :this("dbo") { }
        public BedConfigConfiguration(string schema)
        {
            ToTable(schema + ".BedConfig");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("BedConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BedLengthId).HasColumnName("BedLengthId").IsRequired().HasColumnType("int");
            Property(x => x.BedTypeId).HasColumnName("BedTypeId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Ignore(x => x.VehicleToBedConfigCount);
            //Foreign Keys
            HasRequired(a => a.BedLength).WithMany(b => b.BedConfigs).HasForeignKey(c => c.BedLengthId);
            HasRequired(a => a.BedType).WithMany(b => b.BedConfigs).HasForeignKey(c => c.BedTypeId);
        }
    }
}
