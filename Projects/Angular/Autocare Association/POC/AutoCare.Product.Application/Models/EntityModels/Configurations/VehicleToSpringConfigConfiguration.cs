using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToSpringConfigConfiguration : EntityTypeConfiguration<VehicleToSpringConfig>
    {
        public VehicleToSpringConfigConfiguration()
            : this("dbo")
        {
        }

        public VehicleToSpringConfigConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToSpringConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("VehicleToSpringConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.SpringConfigId).HasColumnName("SpringConfigId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);

            // Foreign keys
            HasRequired(a => a.SpringConfig).WithMany(b => b.VehicleToSpringConfigs).HasForeignKey(c => c.SpringConfigId);
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToSpringConfigs).HasForeignKey(c => c.VehicleId);
        }
    }
}
