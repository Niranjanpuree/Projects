namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FileSizeAddedToAttachments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "FileSize", c => c.Long(nullable: false));
            AddColumn("dbo.AttachmentsStaging", "FileSize", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentsStaging", "FileSize");
            DropColumn("dbo.Attachments", "FileSize");
        }
    }
}
