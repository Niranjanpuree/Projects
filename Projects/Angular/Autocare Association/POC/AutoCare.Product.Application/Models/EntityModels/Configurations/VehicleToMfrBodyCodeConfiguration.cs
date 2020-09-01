using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToMfrBodyCodeConfiguration : EntityTypeConfiguration<VehicleToMfrBodyCode>
    {
        public VehicleToMfrBodyCodeConfiguration()
            : this("dbo")
        { }
        public VehicleToMfrBodyCodeConfiguration(string schema)
        {
            ToTable(schema + ".VehicleToMfrBodyCode");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("VehicleToMfrBodyCodeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName("VehicleId").IsRequired().HasColumnType("int");
            Property(x => x.MfrBodyCodeId).HasColumnName("MfrBodyCodeID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            //for foreign keys
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToMfrBodyCodes).HasForeignKey(c => c.VehicleId);
            HasRequired(a => a.MfrBodyCode).WithMany(b => b.VehicleToMfrBodyCodes).HasForeignKey(c => c.MfrBodyCodeId);
        }
    }
}
