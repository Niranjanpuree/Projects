namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ColumnrenamedToDirectoryPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "DirectoryPath", c => c.String(nullable: false, maxLength: 1024));
            AddColumn("dbo.AttachmentsStaging", "DirectoryPath", c => c.String(nullable: false, maxLength: 1024));
            DropColumn("dbo.Attachments", "FilePath");
            DropColumn("dbo.AttachmentsStaging", "FilePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttachmentsStaging", "FilePath", c => c.String(nullable: false, maxLength: 1024));
            AddColumn("dbo.Attachments", "FilePath", c => c.String(nullable: false, maxLength: 1024));
            DropColumn("dbo.AttachmentsStaging", "DirectoryPath");
            DropColumn("dbo.Attachments", "DirectoryPath");
        }
    }
}
