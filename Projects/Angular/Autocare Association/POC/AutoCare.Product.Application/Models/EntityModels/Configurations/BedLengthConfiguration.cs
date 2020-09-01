using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BedLengthConfiguration : EntityTypeConfiguration<BedLength>
    {
        public BedLengthConfiguration()
            : this("dbo")
        {
        }

        public BedLengthConfiguration(string schema)
        {
            ToTable(schema + ".BedLength");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BedLengthId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Length).HasColumnName("BedLength").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.BedLengthMetric).HasColumnName("BedLengthMetric").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToBedConfigCount);
            Ignore(x => x.BedConfigCount);
        }
    }
}
