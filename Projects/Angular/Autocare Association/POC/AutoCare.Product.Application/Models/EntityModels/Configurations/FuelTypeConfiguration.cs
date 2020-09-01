using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class FuelTypeConfiguration : EntityTypeConfiguration<FuelType>
    {
        public FuelTypeConfiguration()
            : this("dbo")
        {
        }

        public FuelTypeConfiguration(string schema)
        {
            ToTable("FuelType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"FuelTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelTypeName).HasColumnName(@"FuelTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Ignore(x => x.VehicleToEngineConfigCount);
            Ignore(x => x.EngineConfigCount);
        }
    }

}
