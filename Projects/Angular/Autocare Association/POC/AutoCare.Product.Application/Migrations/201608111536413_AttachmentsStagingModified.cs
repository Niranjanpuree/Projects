namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentsStagingModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "FileName", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.Attachments", "AzureContainerName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.AttachmentsStaging", "FileName", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.AttachmentsStaging", "AzureContainerName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Attachments", "OriginalFileName");
            DropColumn("dbo.AttachmentsStaging", "OriginalFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttachmentsStaging", "OriginalFileName", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.Attachments", "OriginalFileName", c => c.String(nullable: false, maxLength: 512));
            DropColumn("dbo.AttachmentsStaging", "AzureContainerName");
            DropColumn("dbo.AttachmentsStaging", "FileName");
            DropColumn("dbo.Attachments", "AzureContainerName");
            DropColumn("dbo.Attachments", "FileName");
        }
    }
}
