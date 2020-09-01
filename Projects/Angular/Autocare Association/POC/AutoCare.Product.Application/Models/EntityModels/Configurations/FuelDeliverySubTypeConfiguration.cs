using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelDeliverySubTypeConfiguration : EntityTypeConfiguration<FuelDeliverySubType>
    {
        public FuelDeliverySubTypeConfiguration()
            : this("dbo")
        {
        }

        public FuelDeliverySubTypeConfiguration(string schema)
        {
            ToTable("FuelDeliverySubType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelDeliverySubTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelDeliverySubTypeName).HasColumnName(@"FuelDeliverySubTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FuelDeliveryConfigCount);
        }
    }
}
