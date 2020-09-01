using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BedTypeConfiguration : EntityTypeConfiguration<BedType>
    {
        public BedTypeConfiguration()
            : this("dbo")
        {
        }

        public BedTypeConfiguration(string schema)
        {
            ToTable(schema + ".BedType");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("BedTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("BedTypeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BedConfigCount);
            Ignore(x => x.VehicleToBedConfigCount);

        }
    }
}
