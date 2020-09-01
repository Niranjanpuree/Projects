using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class CommentsStagingConfiguration: System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CommentsStaging>
    {
        public CommentsStagingConfiguration(): this("dbo")
        {

        }

        public CommentsStagingConfiguration(string schema)
        {
            ToTable("CommentsStaging");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("CommentId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.AddedBy).HasColumnName("AddedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Comment).HasColumnName("Comment").IsRequired().HasColumnType("varchar(max)");
            Property(x => x.CreatedDatetime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequest)
               .WithMany(t => t.CommentsStagings)
               .HasForeignKey(t => t.ChangeRequestId);
        }
    }
}
