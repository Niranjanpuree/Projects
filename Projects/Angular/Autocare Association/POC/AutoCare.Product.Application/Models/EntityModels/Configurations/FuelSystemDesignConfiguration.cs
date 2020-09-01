using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelSystemDesignConfiguration : EntityTypeConfiguration<FuelSystemDesign>
    {
        public FuelSystemDesignConfiguration()
            : this("dbo")
        {
        }

        public FuelSystemDesignConfiguration(string schema)
        {
            ToTable("FuelSystemDesign", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelSystemDesignID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelSystemDesignName).HasColumnName(@"FuelSystemDesignName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FuelDeliveryConfigCount);
        }
    }
}
