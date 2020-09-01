using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class DriveTypeConfiguration : EntityTypeConfiguration<DriveType>
    {
        public DriveTypeConfiguration()
            : this("dbo")
        {
        }

        public DriveTypeConfiguration(string schema)
        {
            ToTable(schema + ".DriveType");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("DriveTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("DriveTypeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToDriveTypeCount);

        }
    }
}
