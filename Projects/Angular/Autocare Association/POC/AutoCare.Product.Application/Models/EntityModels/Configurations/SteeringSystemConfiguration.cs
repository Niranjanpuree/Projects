using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SteeringSystemConfiguration : EntityTypeConfiguration<SteeringSystem>
    {
        public SteeringSystemConfiguration()
            :this("dbo")
        {
        }

        public SteeringSystemConfiguration(string schema)
        {
            ToTable(schema + ".SteeringSystem");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SteeringSystemId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("SteeringSystemName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.SteeringConfigCount);
            Ignore(x => x.VehicleToSteeringConfigCount);
        }
    }
}
