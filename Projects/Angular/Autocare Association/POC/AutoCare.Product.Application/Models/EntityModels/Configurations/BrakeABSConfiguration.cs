using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BrakeABSConfiguration: EntityTypeConfiguration<BrakeABS>
    {
        public BrakeABSConfiguration()
            :this("dbo")
        {
        }

        public BrakeABSConfiguration(string schema)
        {
            ToTable(schema + ".BrakeABS");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BrakeABSId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("BrakeABSName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BrakeConfigCount);
            Ignore(x => x.VehicleToBrakeConfigCount);
        }
    }
}
