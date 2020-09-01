using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToDriveTypeConfiguration : EntityTypeConfiguration<VehicleToDriveType>
    {
        public VehicleToDriveTypeConfiguration()
            : this("dbo")
        { }
        public VehicleToDriveTypeConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToDriveType");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("VehicleToDriveTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.DriveTypeId).HasColumnName("DriveTypeID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            //for foreign keys
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToDriveTypes).HasForeignKey(c => c.VehicleId);
            HasRequired(a => a.DriveType).WithMany(b => b.VehicleToDriveTypes).HasForeignKey(c => c.DriveTypeId);
        }
    }
}
