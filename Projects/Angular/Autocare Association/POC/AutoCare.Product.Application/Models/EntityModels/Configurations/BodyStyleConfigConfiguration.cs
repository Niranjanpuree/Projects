using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class BodyStyleConfigConfiguration : EntityTypeConfiguration<BodyStyleConfig>
    {
        public BodyStyleConfigConfiguration()
            :this("dbo") { }
        public BodyStyleConfigConfiguration(string schema)
        {
            ToTable(schema + ".BodyStyleConfig");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("BodyStyleConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BodyNumDoorsId).HasColumnName("BodyNumDoorsId").IsRequired().HasColumnType("int");
            Property(x => x.BodyTypeId).HasColumnName("BodyTypeId").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Ignore(x => x.VehicleToBodyStyleConfigCount);
            //Foreign Keys
            HasRequired(a => a.BodyNumDoors).WithMany(b => b.BodyStyleConfigs).HasForeignKey(c => c.BodyNumDoorsId);
            HasRequired(a => a.BodyType).WithMany(b => b.BodyStyleConfigs).HasForeignKey(c => c.BodyTypeId);
        }
    }
}
