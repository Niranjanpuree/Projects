using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionNumSpeedsConfiguration : EntityTypeConfiguration<TransmissionNumSpeeds>
    {
        public TransmissionNumSpeedsConfiguration()
            : this("dbo")
        {
        }

        public TransmissionNumSpeedsConfiguration(string schema)
        {
            ToTable("TransmissionNumSpeeds", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionNumSpeedsID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionNumberOfSpeeds).HasColumnName(@"TransmissionNumSpeeds").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(3);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.TransmissionBaseCount);
        }
    }
}
