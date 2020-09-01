using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class AttachmentsConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Attachments>
    {
        public AttachmentsConfiguration() : this("dbo")
        {

        }

        public AttachmentsConfiguration(string schema)
        {
            ToTable(schema + ".Attachments");
            HasKey(x => x.AttachmentId);

            Property(x => x.AttachmentId).HasColumnName("AttachmentId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.FileName).HasColumnName("FileName").IsRequired().HasColumnType("nvarchar").HasMaxLength(512);
            Property(x => x.FileExtension).HasColumnName("FileExtension").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.AzureContainerName).HasColumnName("AzureContainerName").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DirectoryPath).HasColumnName("DirectoryPath").IsRequired().HasColumnType("nvarchar").HasMaxLength(1024);
            Property(x => x.AttachedBy).HasColumnName("AttachedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.ContentType).HasColumnName("ContentType").IsRequired().HasColumnType("varchar").HasMaxLength(255);
            Property(x => x.FileSize).HasColumnName("FileSize").IsRequired().HasColumnType("bigint");
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");

            Ignore(x => x.FileStatus);

            //Relationship
            HasRequired(t => t.ChangeRequestStore)
               .WithMany(t => t.Attachments)
               .HasForeignKey(t => t.ChangeRequestId);
        }

    }
}