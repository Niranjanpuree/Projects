using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToWheelBaseConfiguration : EntityTypeConfiguration<VehicleToWheelBase>
    {
        public VehicleToWheelBaseConfiguration()
            : this("dbo")
        { }
        public VehicleToWheelBaseConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToWheelBase");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("VehicleToWheelBaseID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.WheelBaseId).HasColumnName("WheelBaseId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            //for foreign keys
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToWheelBases).HasForeignKey(c => c.VehicleId);
            HasRequired(a => a.WheelBase).WithMany(b => b.VehicleToWheelBases).HasForeignKey(c => c.WheelBaseId);
        }
    }
}
