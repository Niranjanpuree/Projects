using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class MfrBodyCodeConfiguration : EntityTypeConfiguration<MfrBodyCode>
    {
        public MfrBodyCodeConfiguration()
            : this("dbo")
        {
        }

        public MfrBodyCodeConfiguration(string schema)
        {
            ToTable(schema + ".MfrBodyCode");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("MfrBodyCodeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("MfrBodyCodeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToMfrBodyCodeCount);

        }
    }
}
