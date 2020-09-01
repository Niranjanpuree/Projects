using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelDeliveryTypeConfiguration : EntityTypeConfiguration<FuelDeliveryType>
    {
        public FuelDeliveryTypeConfiguration()
            : this("dbo")
        {
        }

        public FuelDeliveryTypeConfiguration(string schema)
        {
            ToTable("FuelDeliveryType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelDeliveryTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelDeliveryTypeName).HasColumnName(@"FuelDeliveryTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FuelDeliveryConfigCount);
        }
    }
}
