using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ModelConfiguration: EntityTypeConfiguration<Model>
    {
        public ModelConfiguration()
            :this("dbo")
        {
        }

        public ModelConfiguration(string schema)
        {
            ToTable(schema + ".Model");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("ModelId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("ModelName").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(t => t.VehicleTypeId).HasColumnName("VehicleTypeId").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BaseVehicleCount);
            Ignore(x => x.VehicleCount);

            // Relationships
            HasRequired(t => t.VehicleType)
                .WithMany(t => t.Models)
                .HasForeignKey(d => d.VehicleTypeId);
        }
    }
}
