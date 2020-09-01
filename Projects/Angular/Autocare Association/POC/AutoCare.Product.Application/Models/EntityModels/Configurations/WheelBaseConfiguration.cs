using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class WheelBaseConfiguration : EntityTypeConfiguration<WheelBase>
    {
        public WheelBaseConfiguration()
            : this("dbo")
        {
        }

        public WheelBaseConfiguration(string schema)
        {
            ToTable(schema + ".WheelBase");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("WheelBaseID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Base).HasColumnName("WheelBase").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.WheelBaseMetric).HasColumnName("WheelBaseMetric").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToWheelBaseCount);
        }
    }
}
