using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleTypeConfiguration: EntityTypeConfiguration<VehicleType>
    {
        public VehicleTypeConfiguration()
            :this("dbo")
        {

        }

        public VehicleTypeConfiguration(string schema)
        {
            ToTable(schema + ".VehicleType");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("VehicleTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("VehicleTypeName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(t => t.VehicleTypeGroupId).HasColumnName("VehicleTypeGroupId").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.ModelCount);
            Ignore(x => x.BaseVehicleCount);
            Ignore(x => x.VehicleCount);

            // Relationships
            HasRequired(t => t.VehicleTypeGroup)
                .WithMany(t => t.VehicleTypes)
                .HasForeignKey(d => d.VehicleTypeGroupId);
        }
    }
}
