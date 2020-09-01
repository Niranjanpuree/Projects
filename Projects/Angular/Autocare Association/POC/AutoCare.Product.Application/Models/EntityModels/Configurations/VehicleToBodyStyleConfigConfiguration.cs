using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToBodyStyleConfigConfiguration : EntityTypeConfiguration<VehicleToBodyStyleConfig>
    {
        public VehicleToBodyStyleConfigConfiguration()
            :this("dbo")
        { }
        public VehicleToBodyStyleConfigConfiguration( string schema)
        {
            ToTable(schema + ".VehicleToBodyStyleConfig");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("VehicleToBodyStyleConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.BodyStyleConfigId).HasColumnName("BodyStyleConfigId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            //for foreign keys
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToBodyStyleConfigs).HasForeignKey(c => c.VehicleId);
            HasRequired(a => a.BodyStyleConfig).WithMany(b => b.VehicleToBodyStyleConfigs).HasForeignKey(c => c.BodyStyleConfigId);
        }
    }
}
