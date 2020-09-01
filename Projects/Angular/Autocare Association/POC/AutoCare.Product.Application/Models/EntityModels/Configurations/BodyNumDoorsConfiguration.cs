using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BodyNumDoorsConfiguration : EntityTypeConfiguration<BodyNumDoors>
    {
        public BodyNumDoorsConfiguration()
            : this("dbo")
        {
        }

        public BodyNumDoorsConfiguration(string schema)
        {
            ToTable(schema + ".BodyNumDoors");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("BodyNumDoorsId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NumDoors).HasColumnName("BodyNumDoors").IsRequired().HasColumnType("char").HasMaxLength(3);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BodyStyleConfigCount);
            Ignore(x => x.VehicleToBodyStyleConfigCount);
        }
    }
}
