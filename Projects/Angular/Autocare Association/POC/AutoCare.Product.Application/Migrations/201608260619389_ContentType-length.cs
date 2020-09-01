namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ContentTypelength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attachments", "ContentType", c => c.String(nullable: false, maxLength: 255, unicode: false));
            AlterColumn("dbo.AttachmentsStaging", "ContentType", c => c.String(nullable: false, maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AttachmentsStaging", "ContentType", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Attachments", "ContentType", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
