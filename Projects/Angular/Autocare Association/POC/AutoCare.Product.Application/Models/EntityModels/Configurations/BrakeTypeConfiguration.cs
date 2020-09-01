using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BrakeTypeConfiguration: EntityTypeConfiguration<BrakeType>
    {
        public BrakeTypeConfiguration()
            :this("dbo")
        {
        }

        public BrakeTypeConfiguration(string schema)
        {
            ToTable(schema + ".BrakeType");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BrakeTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("BrakeTypeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FrontBrakeConfigCount);
            Ignore(x => x.RearBrakeConfigCount);
            Ignore(x => x.VehicleToBrakeConfigCount);
        }
    }
}
