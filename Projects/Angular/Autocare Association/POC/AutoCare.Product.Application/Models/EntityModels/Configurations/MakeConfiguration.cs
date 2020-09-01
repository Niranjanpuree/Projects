using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class MakeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Make>
    {
        public MakeConfiguration()
           : this("dbo")
        {
        }

        public MakeConfiguration(string schema)
        {
            ToTable(schema + ".Make");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("MakeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("MakeName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BaseVehicleCount);
            Ignore(x => x.VehicleCount);

            
        }
    }
}