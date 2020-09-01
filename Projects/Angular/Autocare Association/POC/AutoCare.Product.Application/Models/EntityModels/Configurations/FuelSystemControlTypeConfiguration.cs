using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelSystemControlTypeConfiguration : EntityTypeConfiguration<FuelSystemControlType>
    {
        public FuelSystemControlTypeConfiguration()
            : this("dbo")
        {
        }

        public FuelSystemControlTypeConfiguration(string schema)
        {
            ToTable("FuelSystemControlType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelSystemControlTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelSystemControlTypeName).HasColumnName(@"FuelSystemControlTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FuelDeliveryConfigCount);
        }
    }
}
