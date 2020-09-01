using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SteeringTypeConfiguration : EntityTypeConfiguration<SteeringType>
    {
        public SteeringTypeConfiguration()
            :this("dbo")
        {
        }

        public SteeringTypeConfiguration(string schema)
        {
            ToTable(schema + ".SteeringType");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SteeringTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("SteeringTypeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.SteeringConfigCount);
           
            Ignore(x => x.VehicleToSteeringConfigCount);
        }
    }
}
