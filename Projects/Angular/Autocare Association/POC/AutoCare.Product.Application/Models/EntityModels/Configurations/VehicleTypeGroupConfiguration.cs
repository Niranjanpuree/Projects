using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleTypeGroupConfiguration: EntityTypeConfiguration<VehicleTypeGroup>
    {
        public VehicleTypeGroupConfiguration()
            :this("dbo")
        {
        }

        public VehicleTypeGroupConfiguration(string schema)
        {
            ToTable(schema + ".VehicleTypeGroup");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("VehicleTypeGroupId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("VehicleTypeGroupName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleTypeCount);
        }
    }
}
