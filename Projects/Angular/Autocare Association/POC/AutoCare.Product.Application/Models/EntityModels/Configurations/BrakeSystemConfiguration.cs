using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BrakeSystemConfiguration: EntityTypeConfiguration<BrakeSystem>
    {
        public BrakeSystemConfiguration()
            :this("dbo")
        {
        }

        public BrakeSystemConfiguration(string schema)
        {
            ToTable(schema + ".BrakeSystem");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BrakeSystemId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("BrakeSystemName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BrakeConfigCount);
            Ignore(x => x.VehicleToBrakeConfigCount);
        }
    }
}
