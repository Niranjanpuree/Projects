using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BaseVehicleConfiguration: EntityTypeConfiguration<BaseVehicle>
    {
        public BaseVehicleConfiguration()
            :this("dbo")
        {
        }

        public BaseVehicleConfiguration(string schema)
        {
            ToTable(schema + ".BaseVehicle");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BaseVehicleId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MakeId).HasColumnName("MakeId").IsRequired();
            Property(x => x.ModelId).HasColumnName("ModelId").IsRequired();
            Property(x => x.YearId).HasColumnName("YearId").HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleCount);

            // Relationships
            HasRequired(t => t.Make)
                .WithMany(t => t.BaseVehicles)
                .HasForeignKey(d => d.MakeId);

            HasRequired(t => t.Model)
                .WithMany(t => t.BaseVehicles)
                .HasForeignKey(d => d.ModelId);

            HasRequired(t => t.Year)
                .WithMany(t => t.BaseVehicles)
                .HasForeignKey(d => d.YearId);
        }
    }
}
