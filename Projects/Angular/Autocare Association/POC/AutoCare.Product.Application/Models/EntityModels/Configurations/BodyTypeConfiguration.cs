using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BodyTypeConfiguration : EntityTypeConfiguration<BodyType>
    {
        public BodyTypeConfiguration()
            : this("dbo")
        {
        }

        public BodyTypeConfiguration(string schema)
        {
            ToTable(schema + ".BodyType");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("BodyTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("BodyTypeName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BodyStyleConfigCount);
            Ignore(x => x.VehicleToBodyStyleConfigCount);

        }
    }
}
